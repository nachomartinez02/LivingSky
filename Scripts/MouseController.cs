using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class MouseController : MonoBehaviour
{
    public float speed;
    public List<GameObject> characterPrefabs;
    [HideInInspector] public List<CharacterInfo> characters = new List<CharacterInfo>();
    private int activeCharacterIndex = 0;

    private PathFinder pathFinder;
    private RangeFinder rangeFinder;
    private List<OverlayTile> path = new List<OverlayTile>();
    private List<OverlayTile> inRangeTiles = new List<OverlayTile>();

    public List<Vector2Int> initialCharacterPositions;
    public List<int> movementCharacterRange;
    public List<Button> buttonsListCharacters;
    public List<int> attackCharacterRange;

    private IAInfo personajeatacado;

    public Button btn_next_round;
    public Button btn_move;
    public Button btn_attack;
    public Button btn_return;

    public GameObject canvas_change_round;
    public GameObject canvas_chooseCharacter;
    public GameObject canvas_chooseThingToDo;

    public GameObject canvas_Win;
    public GameObject canvas_Defeat;

    public bool canAttack = true;
    public bool canMove = true;
    private int activeButtonIndex = -1;

    private float lastClickTime = 0f;

    public Color rangeMoveColor;
    public Color rangeAttackColor;
    public Color pointOnAttackColor;

    public Color activeColorButton;
    public Color inactiveColorButton;

    private IAController iaController;
    public GameObject torretaObjeto;

    public GameObject iAController;

    private Powerups scriptPowerups;

    public GameObject panelMainScene;
    public GameObject panelChangeRound;
    private void Start()
    {
        
        pathFinder = new PathFinder();
        rangeFinder = new RangeFinder();

        iaController = iAController.GetComponent<IAController>();

        scriptPowerups = torretaObjeto.GetComponent<Powerups>();

        GameObject charactersContainer = GameObject.Find("Characters"); // Encuentra el objeto "Characters"

        for (int i = 0; i < characterPrefabs.Count; i++)
        {
            Vector3 initialWorldPosition =
                MapManager.Instance.GetWorldPositionFromTileLocation(initialCharacterPositions[i]);
            CharacterInfo newCharacter = Instantiate(characterPrefabs[i], initialWorldPosition, Quaternion.identity)
                .GetComponent<CharacterInfo>();

            OverlayTile initialTile = MapManager.Instance.GetTileFromTileLocation(initialCharacterPositions[i]);

            if (initialTile != null)
            {
                PositionCharacterOnTile(newCharacter, initialTile);
            }
            else
            {
                Debug.LogError("Tile inicial no encontrado para la posici�n: " + initialCharacterPositions[i]);
            }

            characters.Add(newCharacter);

            // Mueve el personaje al contenedor "Characters"
            if (charactersContainer != null)
            {
                newCharacter.transform.parent = charactersContainer.transform;
            }
        }

        for (int i = 0; i < buttonsListCharacters.Count; i++)
        {
            int characterIndex = i;
            buttonsListCharacters[i].GetComponent<Image>().color = activeColorButton;
            buttonsListCharacters[i].onClick.AddListener(() => ActivateChooseThingToDoPanel(characterIndex));

        }

        publicVariables.indexNextRound = characters.Count;
        Debug.Log("START: " + publicVariables.indexNextRound);
    }

    void Update()
    {
        if (iaController.excluidos.Count == 3)
        {
            canvas_Win.SetActive(true);
            canvas_chooseCharacter.SetActive(false);
            canvas_chooseThingToDo.SetActive(false);
        }
        btn_next_round.onClick.AddListener(SetBtnChangeRound);


        if (publicVariables.movementsMouseController < publicVariables.indexNextRound)
        {
            var focusedTileHit = GetFocusedOnTile();

            if (focusedTileHit.HasValue)
            {
                OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
                if(IsValidMove(overlayTile))
                {
                    transform.position = overlayTile.transform.position;
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = 25;

                    if (Input.GetMouseButtonDown(0) && canMove)
                    {
                        if (Time.time - lastClickTime > 0.2f)
                        {
                            lastClickTime = Time.time;
                            CharacterInfo currentCharacter = characters[activeCharacterIndex];

                            if (currentCharacter == null)
                            {
                                Debug.Log("Error move");
                            }
                            else
                            {
                                path = pathFinder.FindPath(currentCharacter.activeTile, overlayTile, inRangeTiles);
                                Debug.Log("La variable es " + publicVariables.movementsMouseController);
                                canvas_chooseThingToDo.SetActive(false);


                                characters[activeCharacterIndex].BorrarCuadroTexto();
                            }
                        }
                    }
                }

                if (IsValidAtack(overlayTile))
                {

                    transform.position = overlayTile.transform.position;
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = 25;

                    if (Input.GetMouseButtonDown(0) && canAttack)
                    {

                        if (Time.time - lastClickTime > 0.2f)
                        {
                            lastClickTime = Time.time;
                            CharacterInfo currentCharacter = characters[activeCharacterIndex];

                            if (currentCharacter == null)
                            {
                                Debug.Log("Error attack");
                            }
                            else
                            {
                                publicVariables.movementsMouseController++;
                                currentCharacter.EmpezarAtaque();
                                personajeatacado.Pupa(currentCharacter.GetAtaque());
                                
                                foreach (var item in inRangeTiles)
                                {
                                    item.HideTile();
                                }
                                for (int i = 0; i < buttonsListCharacters.Count; i++)
                                {
                                    if (activeButtonIndex == i )
                                    {
                                        buttonsListCharacters[i].interactable = false;
                                        buttonsListCharacters[i].GetComponent<Image>().color = inactiveColorButton;
                                    }
                                }

                                if (personajeatacado.vida <= 0)
                                {
                                    canvas_Defeat.SetActive(true);
                                    canvas_chooseCharacter.SetActive(false);
                                    canvas_chooseThingToDo.SetActive(false);
                                }

                                canvas_chooseCharacter.SetActive(true);
                                canvas_chooseThingToDo.SetActive(false);
                                canAttack = false;
                                publicVariables.rangeTowerAttack = true;
                                characters[activeCharacterIndex].BorrarCuadroTexto();
                            }
                        }

                    }

                }

            }


            if (path.Count > 0)
            {
                float step = speed * Time.deltaTime;
                Vector2 targetPosition = path[0].transform.position;

                characters[activeCharacterIndex].transform.position = Vector2.MoveTowards(
                    characters[activeCharacterIndex].transform.position, targetPosition, step);

                if (Vector2.Distance(characters[activeCharacterIndex].transform.position, targetPosition) < 0.001f)
                {
                    PositionCharacterOnTile(characters[activeCharacterIndex], path[0]);
                    path.RemoveAt(0);
                }

                if (path.Count == 0)
                {
                    foreach (var item in inRangeTiles)
                    {
                        item.HideTile();
                    }

                    if (publicVariables.movementsMouseController < publicVariables.indexNextRound)
                    {
                        publicVariables.movementsMouseController++;
                        canMove = true;
                        activeCharacterIndex = (activeCharacterIndex + 1) % characters.Count;

                        for (int i = 0; i < buttonsListCharacters.Count; i++)
                        {
                            if (activeButtonIndex == i)
                            {
                                buttonsListCharacters[i].interactable = false;
                                buttonsListCharacters[i].GetComponent<Image>().color = inactiveColorButton;
                            }
                        }
                        publicVariables.rangeTower = true;
                        canvas_chooseCharacter.SetActive(true);
                        canAttack = false;

                        publicVariables.rangeYellow = true;
                        publicVariables.rangeRed = true;
                    }
                }
            }
        }
        if (publicVariables.movementsMouseController == publicVariables.indexNextRound && path.Count == 0)
        {
            canvas_change_round.SetActive(true);
            canvas_chooseCharacter.SetActive(false);
        }
        
    }
    private void ActivateChooseThingToDoPanel(int characterIndex)
    {
        activeButtonIndex = characterIndex;

        btn_move.onClick.AddListener(() => MovementCharacter(characterIndex));
        btn_move.onClick.AddListener(() => canvas_chooseCharacter.SetActive(false));
        btn_move.onClick.AddListener(() => publicVariables.rangeTower = false);
        btn_move.onClick.AddListener(() => characters[activeCharacterIndex].BorrarCuadroTexto());
        btn_move.onClick.AddListener(() => publicVariables.rangeTowerAttack = true);

        btn_attack.onClick.AddListener(() => ShowAttackRange(characterIndex));
        btn_attack.onClick.AddListener(() => canAttack = true);
        btn_attack.onClick.AddListener(() => canvas_chooseCharacter.SetActive(false));
        btn_attack.onClick.AddListener(() => characters[activeCharacterIndex].BorrarCuadroTexto());
        btn_attack.onClick.AddListener(() => publicVariables.rangeTowerAttack = false);
        btn_attack.onClick.AddListener(() => publicVariables.rangeTower = true);

        btn_return.onClick.AddListener(() => DontShowAttackRange(characterIndex));

        //btn_return.onClick.AddListener(() => scriptTorreta.ShowMovementRange());

        btn_return.onClick.AddListener(() => canAttack = false);
        btn_return.onClick.AddListener(() => canvas_chooseCharacter.SetActive(true));
        btn_return.onClick.AddListener(() => publicVariables.rangeTowerAttack = true);
        btn_return.onClick.AddListener(() => publicVariables.rangeTower = true);
        canvas_chooseThingToDo.SetActive(true);

    }
    private void MovementCharacter(int characterIndex)
    {
        canMove = true;
        ShowMovementRange(characterIndex);
    }
    private void SetBtnChangeRound()
    {
        if (publicVariables.movementsMouseController == publicVariables.indexNextRound)
        {
            RandomPower();
            canMove = true;
            publicVariables.movementsMouseController = 0;
            canvas_change_round.SetActive(false);
            canvas_chooseCharacter.SetActive(true);

            StartCoroutine(ActiveButtons());

            publicVariables.myTurn = true;
            publicVariables.rangeAttackTower = true;

            
        }
    }

    private  IEnumerator ActiveButtons()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < buttonsListCharacters.Count; i++)
        {
            if (!iaController.excluidos.Contains(i))
            {
                buttonsListCharacters[i].interactable = true;
                buttonsListCharacters[i].GetComponent<Image>().color = activeColorButton;
                Debug.Log(i);
            }
            else
            {
                buttonsListCharacters[i].interactable = false;
                buttonsListCharacters[i].GetComponent<Image>().color = inactiveColorButton;
            }

        }
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }

    private void PositionCharacterOnTile(CharacterInfo character, OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = 30;
        character.activeTile = tile;
    }

    private void ShowMovementRange(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < characters.Count)
        {
            int currentMovementRange = movementCharacterRange[characterIndex];

            foreach (var item in inRangeTiles)
            {
                item.HideTile();
            }

            List<Vector2Int> occupiedPositions = new List<Vector2Int>();

            for (int i = 0; i < characters.Count; i++)
            {
                if (i != characterIndex && !iaController.excluidos.Contains(i))
                {
                    occupiedPositions.Add(characters[i].activeTile.gridLocation);
                }
            }

            inRangeTiles = rangeFinder.GetTilesInRange(characters[characterIndex].activeTile, currentMovementRange);

            foreach (var item in inRangeTiles)
            {
                if (occupiedPositions.Contains(item.gridLocation))
                {
                    item.HideTile();
                }
                else
                {
                    item.ShowTile();
                    item.GetComponent<SpriteRenderer>().color = rangeMoveColor;
                }
            }

            activeCharacterIndex = characterIndex;
        }
    }

    private void ShowAttackRange(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < characters.Count)
        {
            int attackRange = attackCharacterRange[characterIndex];

            foreach (var item in inRangeTiles)
            {
                item.HideTile();
            }

            List<Vector2Int> occupiedPositions = new List<Vector2Int>();

            occupiedPositions.Add(iaController.characterIA.activeTileIA.gridLocation);
            inRangeTiles = rangeFinder.GetTilesInRange(characters[characterIndex].activeTile, attackRange);

            foreach (var item in inRangeTiles)
            {

                if (occupiedPositions.Contains(item.gridLocation))
                {
                    item.ShowTile();
                    item.GetComponent<SpriteRenderer>().color = pointOnAttackColor;
                }
                else
                {
                    item.ShowTile();
                    item.GetComponent<SpriteRenderer>().color = rangeAttackColor;
                }

            }

            activeCharacterIndex = characterIndex;
        }

        canMove = false;
    }
    private void DontShowAttackRange(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < characters.Count)
        {
            int attackRange = attackCharacterRange[characterIndex];

            foreach (var item in inRangeTiles)
            {
                item.HideTile();
            }

            inRangeTiles = rangeFinder.GetTilesInRange(characters[characterIndex].activeTile, attackRange);


        }

        canMove = false;
    }
    private bool IsValidMove(OverlayTile overlayTile)
    {
        foreach (var c in characters)
        {
            if (c.activeTile == overlayTile && iaController.excluidos.Contains(c.index))
            {
                return true;
            }
        }
       

        if (!inRangeTiles.Contains(overlayTile))
        {
            return false;
        }

        foreach (var character in characters)
        {
            if (character != characters[activeCharacterIndex] && character.activeTile == overlayTile)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsValidAtack(OverlayTile overlayTile)
    {
        bool haceDa�o = false;
        // Verifica si la casilla esta dentro del rango
        if (!inRangeTiles.Contains(overlayTile))
        {
            return false;
        }

        // Verifica si la casilla esta ocupada por otro personaje
        if (iaController.characterIA.activeTileIA == overlayTile)
        {
            personajeatacado = iaController.characterIA;
            haceDa�o = true;
        }

        return haceDa�o;
    }

    public List<OverlayTile> GetTilesInRange()
    {
        return inRangeTiles.ToList();
    }

    private void RandomPower()
    {
        //primero 80% de que cree un objeto

        int posibilidadUno = Random.Range(0, 9 + 1);
        int powerGenerado = Random.Range(0, 1 + 1);
        if (posibilidadUno <= 5)
        {
            if (publicVariables.healthInMap == false)
            {
                scriptPowerups.CrearPower(0);
                publicVariables.healthInMap = true;
            }
            else if (powerGenerado == 1 && publicVariables.swordInMap == false)
            {
                scriptPowerups.CrearPower(powerGenerado);
                publicVariables.swordInMap = true;
            }
        }
            

        /*if (posibilidadUno <= 6)
        {
            scriptPowerups.CrearPower(powerGenerado);
            int posibilidadDos = Random.Range(0, 1 + 1);//si despues de eso sale el 1, 50%, se genera el que falte

            if (posibilidadDos == 1)
            {
                if (powerGenerado == 0)
                {
                    scriptPowerups.CrearPower(1);
                }
                else
                {
                    scriptPowerups.CrearPower(0);
                }
            }
        }*/
    }
}