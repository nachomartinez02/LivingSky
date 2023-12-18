using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Powerups : MonoBehaviour
{
    private MovPowers newPowerup;

    public List<GameObject> powerupsPrefabs;

    private BorrarTileAleatorio vacioScript;

    public GameObject cajaPrefab;

    // Start is called before the first frame update
    public void CrearPower(int pu)
    {
        vacioScript = cajaPrefab.GetComponent<BorrarTileAleatorio>();
        GameObject powerUpsContainer = GameObject.Find("PowerUps");

        int minRangeX = 0;
        int maxRangeX = 8;

        int minRangeY = -6;
        int maxRangeY = 2;

        Vector2Int initialPowerupsPositions = new Vector2Int(Random.Range(minRangeX, maxRangeX + 1), Random.Range(minRangeY, maxRangeY + 1));

        Vector3 initialWorldPosition = MapManager.Instance.GetWorldPositionFromTileLocation(initialPowerupsPositions);

        while (vacioScript.ListaCajas.Contains(initialPowerupsPositions))
        {
            initialPowerupsPositions = new Vector2Int(Random.Range(minRangeX, maxRangeX + 1), Random.Range(minRangeY, maxRangeY + 1));

        }

        newPowerup = Instantiate(powerupsPrefabs[pu], initialWorldPosition, Quaternion.identity).GetComponent<MovPowers>();
        vacioScript.ListaCajas.Add(initialPowerupsPositions);

        OverlayTile initialTile = MapManager.Instance.GetTileFromTileLocation(initialPowerupsPositions);
        newPowerup.transform.SetParent(powerUpsContainer.transform);

        if (initialTile != null)
        {
            PositionPowerupOnTile(newPowerup, initialTile);
        }
        else
        {
            Debug.LogError("Tile inicial no encontrado para la posicion: " + initialPowerupsPositions);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PositionPowerupOnTile(MovPowers powerup, OverlayTile tile)
    {
        powerup.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);  
        powerup.GetComponent<SpriteRenderer>().sortingOrder = 30;
        powerup.activeTilePowerups = tile;
    }

}
