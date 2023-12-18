using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using System.Linq;
using System.Xml;

public class IAController : MonoBehaviour
{
    public GameObject characterPrefab;
    public Vector2Int initialPosition;
    public GameObject characterPrefabScript;

    public int currentRangeIAMove;
    public int currentRangeIAAttack;
    public int currentRangeIALook;
    [HideInInspector] public IAInfo characterIA;
    [HideInInspector] public MovPowers movPowers;
    [HideInInspector] public MouseController mouseController;

    private PathFinder pathFinder;
    private RangeFinder rangeFinder;
    private List<OverlayTile> path = new List<OverlayTile>();
    private List<OverlayTile> inRangeTiles = new List<OverlayTile>();
    private List<OverlayTile> yellowRangeTiles = new List<OverlayTile>();

    [HideInInspector] public List<Transform> rangeMovement = new List<Transform>();
    [HideInInspector] public List<Transform> inRangeYellowMovement = new List<Transform>();
    [HideInInspector] public List<int> excluidos = new List<int>();

    [HideInInspector] public List<OverlayTile> redRangeTiles = new List<OverlayTile>();
    [HideInInspector] public List<Transform> inRangeRedMovement = new List<Transform>();

    [HideInInspector] public List<Transform> powerUps = new List<Transform>();
    [HideInInspector] public List<Transform> powerUpsInRange = new List<Transform>();


    public Color rangeColorBlue = new Color(0f, 0f, 1f, 0.5f);
    public Color rangeColorYellow = new Color(1f, 1f, 0f, 0.5f);
    public Color rangeColorRed = new Color(1f, 0f, 0f, 0.5f);


    private bool checkMove = true;
    private object lockObject = new object();
    private void Start()
    {
        rangeFinder = new RangeFinder();
        pathFinder = new PathFinder();
        // Busca el objeto "CharactersIA" en la escena
        GameObject charactersContainer = GameObject.Find("CharactersIA");
        mouseController = characterPrefabScript.GetComponent<MouseController>();
        // Instantiate the character
        Vector3 initialWorldPosition = MapManager.Instance.GetWorldPositionFromTileLocation(initialPosition);
        characterIA = Instantiate(characterPrefab, initialWorldPosition, Quaternion.identity).GetComponent<IAInfo>();
        characterIA.transform.SetParent(charactersContainer.transform);

        OverlayTile initialTile = MapManager.Instance.GetTileFromTileLocation(initialPosition);

        if (initialTile != null)
        {
            PositionCharacterOnTile(characterIA, initialTile);
        }
        else
        {
            Debug.LogError("Initial tile not found for position: " + initialPosition);
        }

        UpdateRangeMovementList();
        UpdatePowerUpsInRangeList();
    }

    void Update()
    {
        UpdateRangeMovementList();
        UpdatePowerUpsInRangeList();

        if (publicVariables.rangeYellow)
        {
            ShowYellowRange();
            ShowMovementRange();
        }
        if (publicVariables.rangeRed)
        {
            ShowRedRange();
        }

        MovementIA();

    }
    private void UpdateRangeMovementList()
    {
        GameObject charactersObject = GameObject.Find("Characters");

        if (charactersObject != null)
        {
            lock (lockObject)
            {
                rangeMovement.Clear();
                foreach (Transform child in charactersObject.transform)
                {
                    rangeMovement.Add(child);
                }
            }
        }
    }
    private void UpdatePowerUpsInRangeList()
    {
        GameObject powerupsContainer = GameObject.Find("PowerUps");

        if (powerupsContainer != null)
        {
            lock (lockObject)
            {
                powerUps.Clear();
                foreach (Transform child in powerupsContainer.transform)
                {
                    powerUps.Add(child);
                }
            }
        }
    }
    public void MovementIA()
    {
        if (checkMove)
        {
            BehaviorNode behaviorTree = ConstructBehaviorTree();
            behaviorTree.Execute();
        }
    }

    public void RandomMove()
    {
        if (checkMove)
        {
            checkMove = false;

            StartCoroutine(MoveIA(GetRandomPositionInMovementRange()));
        }
    }

    public void LookMove()
    {
        if (checkMove && inRangeYellowMovement.Count > 0 && inRangeYellowMovement[0] != null)
        {
            Transform targetTransform = inRangeYellowMovement[0];

            CharacterInfo targetIA = targetTransform.GetComponent<CharacterInfo>();

            StartCoroutine(MoveToPosition(targetIA.activeTile.gridLocation));
        }

    }
    public void PowerUpMove()
    {
        if (checkMove && powerUpsInRange.Count > 0 && powerUpsInRange[0] != null)
        {
            Debug.Log("se mueve");
            Transform targetTransform = powerUpsInRange[0];

            MovPowers  targetMovPowers = targetTransform.GetComponent<MovPowers>();

            StartCoroutine(MoveToPowerUp(targetMovPowers.activeTilePowerups.gridLocation));
        }
    }
    private IEnumerator MoveToPosition(Vector2Int targetPosition)
    {
        if (characterIA.activeTileIA.gridLocation == targetPosition)
        {
            path = pathFinder.FindPath(characterIA.activeTileIA, MapManager.Instance.GetTileFromTileLocation(targetPosition), inRangeTiles);
            yield return StartCoroutine(MoveCharacterOnPath(targetPosition));
        }
        else
        {
            Vector2Int nearestPosition = FindNearestPositionInMovementRange(targetPosition);

            path = pathFinder.FindPath(characterIA.activeTileIA, MapManager.Instance.GetTileFromTileLocation(nearestPosition), inRangeTiles);

            yield return StartCoroutine(MoveCharacterOnPath(nearestPosition));
        }
    }
    private Vector2Int FindNearestPositionInMovementRange(Vector2Int targetPosition)
    {
        List<OverlayTile> validTiles = rangeFinder.GetTilesInRange(characterIA.activeTileIA, 3);

        Vector2Int nearestPosition = characterIA.activeTileIA.gridLocation;
        int minDistance = int.MaxValue;

        foreach (var tile in validTiles)
        {
            int distance = pathFinder.GetManhattenDistance(tile, MapManager.Instance.GetTileFromTileLocation(targetPosition));

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPosition = tile.gridLocation;
            }
        }

        return nearestPosition;
    }
    private IEnumerator MoveIA(Vector2Int targetPosition)
    {
        path = pathFinder.FindPath(characterIA.activeTileIA, MapManager.Instance.GetTileFromTileLocation(targetPosition), inRangeTiles);
        if (path.Count > 0)
        {
            Vector2Int finalPosition = path[path.Count - 1].gridLocation;
            yield return StartCoroutine(MoveCharacterOnPath(finalPosition));
        }

        publicVariables.myTurn = false;
        checkMove = true;
    }
    private IEnumerator MoveCharacterOnPath(Vector2Int targetPosition)
    {
        float journeyLength = 0f;
        float startTime = Time.time;
        float speed = 2f;

        foreach (var tile in path)
        {
            Vector2 startPosition = characterIA.transform.position;
            Vector2 tilePosition = tile.transform.position;
            journeyLength = Vector2.Distance(startPosition, tilePosition);

            while (Vector2.Distance(characterIA.transform.position, tilePosition) > 0.001f)
            {
                if (inRangeRedMovement.Count > 0)
                {
                    yield break;
                }

                float distCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distCovered / journeyLength;

                characterIA.transform.position = Vector2.Lerp(startPosition, tilePosition, fractionOfJourney);

                yield return null;
            }

            PositionCharacterOnTile(characterIA, tile);

            foreach (var item in inRangeTiles)
            {
                item.HideTile();
            }

            yield return new WaitForEndOfFrame();

            publicVariables.myTurn = false;
            checkMove = true;

            startTime = Time.time;
        }
    }
    private BehaviorNode ConstructBehaviorTree()
    {
        return new ActionNode( this, inRangeYellowMovement.Count, inRangeRedMovement.Count, powerUpsInRange.Count);
    }
    private void PositionCharacterOnTile(IAInfo ia, OverlayTile tile)
    {
        ia.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        ia.GetComponent<SpriteRenderer>().sortingOrder = 30;
        ia.activeTileIA = tile;
    }
    private void ShowYellowRange()
    {
        foreach (var item in yellowRangeTiles)
        {
            item.HideTile();
        }

        lock (lockObject)
        {
            yellowRangeTiles = rangeFinder.GetTilesInRange(characterIA.activeTileIA, currentRangeIALook);

            inRangeYellowMovement.Clear();
            powerUpsInRange.Clear();

            var orderedCharacters = rangeMovement
                .Where(characterTransform => yellowRangeTiles.Any(tile =>
                    Vector3.Distance(tile.transform.position, characterTransform.position) < 0.1f))
                .OrderBy(characterTransform => characterTransform.GetComponent<CharacterInfo>().vida);
            var orderedPowerUps = powerUps
                .OrderBy(powerUpTransform => powerUpTransform.name);



            inRangeYellowMovement.AddRange(orderedCharacters);
            powerUpsInRange.AddRange(orderedPowerUps);

            foreach (var item in yellowRangeTiles)
            {
                var charactersCopy = inRangeYellowMovement.ToList();
                var powerupsCopy = powerUpsInRange.ToList();
                item.ShowTile();
                item.GetComponent<SpriteRenderer>().color = rangeColorYellow;

                foreach (var characterTransform in charactersCopy)
                {
                    if (Vector3.Distance(item.transform.position, characterTransform.position) < 0.1f)
                    {
                        break;
                    }
                }
                foreach (var powerUpTransform in powerupsCopy)
                {
                    if (Vector3.Distance(item.transform.position, powerUpTransform.position) < 0.1f)
                    {
                        break;
                    }
                }
            }

            publicVariables.rangeYellow = false;
        }

        UpdateRangeMovementList();
        UpdatePowerUpsInRangeList();
        Debug.Log("asd" + inRangeYellowMovement.Count);
    } 
    private void ShowMovementRange()
    {
        foreach (var item in inRangeTiles)
        {
            if (item != null)
            {
                item.HideTile();
            }
        }

        inRangeTiles = rangeFinder.GetTilesInRange(characterIA.activeTileIA, currentRangeIAMove);

        foreach (var item in inRangeTiles)
        {
            if (item != null)
            {
                item.ShowTile();
                item.GetComponent<SpriteRenderer>().color = rangeColorBlue;
            }
        }
    }
    private void ShowRedRange()
    {
        List<OverlayTile> tilesInRange = mouseController.GetTilesInRange();
        foreach (var item in redRangeTiles)
        {
            item.HideTile();
        }

        lock (lockObject)
        {
            redRangeTiles = rangeFinder.GetTilesInRange(characterIA.activeTileIA, currentRangeIAAttack); // Ajusta el rango de ataque según tus necesidades

            inRangeRedMovement.Clear();

            var orderedCharacters = rangeMovement
                .Where(characterTransform => redRangeTiles.Any(tile => Vector3.Distance(tile.transform.position, characterTransform.position) < 0.1f))
                .OrderBy(characterTransform => characterTransform.GetComponent<CharacterInfo>().vida);

            inRangeRedMovement.AddRange(orderedCharacters);

            foreach (var item in redRangeTiles)
            {
                if (tilesInRange.Contains(item))
                {
                    var charactersCopy = inRangeRedMovement.ToList();
                    if (!publicVariables.rangeTower)
                    {
                        item.ShowTile();
                        item.GetComponent<SpriteRenderer>().color = new Color(1f, 0.4f, 1f, 0.5f);
                    }
                    else if (!publicVariables.rangeTowerAttack)
                    {
                        item.ShowTile();
                        item.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0f, 0f, 0.8f);
                    }
                    else
                    {
                        item.ShowTile();
                        item.GetComponent<SpriteRenderer>().color = rangeColorRed;
                    }

                    foreach (var characterTransform in charactersCopy)
                    {
                        if (Vector3.Distance(item.transform.position, characterTransform.position) < 0.1f)
                        {
                            break;


                        }
                    }
                }
                else
                {
                    item.ShowTile();
                    item.GetComponent<SpriteRenderer>().color = rangeColorRed;
                }
            }

            publicVariables.rangeRed = false;
        }
            
    }
    private Vector2Int GetRandomPositionInMovementRange()
    {
        List<OverlayTile> validTiles = rangeFinder.GetTilesInRange(characterIA.activeTileIA, Mathf.Clamp(currentRangeIAMove, 1, 2));

        validTiles.RemoveAll(tile => tile.gridLocation == characterIA.activeTileIA.gridLocation);

        if (validTiles.Count > 0)
        {
            int randomIndex = Random.Range(0, validTiles.Count);

            return validTiles[randomIndex].gridLocation;
        }

        return characterIA.activeTileIA.gridLocation;
    }
    public void AttackMove()
    {
        if (checkMove)
        {
            Transform targetTransform = inRangeRedMovement[0];
            CharacterInfo targetCharacter = targetTransform.GetComponent<CharacterInfo>();

            characterIA.EmpezarAtaque();

            targetCharacter.Pupa(characterIA.ataque);
            if (targetCharacter.vida <= 0)
            {
                excluidos.Add(targetCharacter.index);
                ShowYellowRange();
                ShowRedRange();
                
                publicVariables.indexNextRound--;
            }
            
            checkMove = true;
            publicVariables.myTurn = false;
        }
    }
    private IEnumerator MoveToPowerUp(Vector2Int powerUpTransform)
    {
        if (characterIA.activeTileIA.gridLocation == powerUpTransform)
        {
            // Character is already on the power-up tile
            yield break;
        }
        else
        {
            Vector2Int nearestPosition = FindNearestPositionInMovementRange(powerUpTransform);

            path = pathFinder.FindPath(characterIA.activeTileIA, MapManager.Instance.GetTileFromTileLocation(nearestPosition), inRangeTiles);

            yield return StartCoroutine(MoveCharacterOnPath(nearestPosition));
        }

        // Perform any additional actions related to reaching the power-up
        // (e.g., pick up the power-up, update variables, etc.)

        // After reaching the power-up, you may want to update the power-ups list
        UpdatePowerUpsInRangeList();

        // Set checkMove and turn variables accordingly
        checkMove = true;
        publicVariables.myTurn = false;
    }
}