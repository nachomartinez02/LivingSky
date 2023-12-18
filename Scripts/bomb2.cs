using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb2 : MonoBehaviour
{
    private bomb bomb;

    public GameObject bombitaa;


    
    
    // Start is called before the first frame update
    public void CrearBomb(CharacterInfo c)
    {
        GameObject powerUpsContainer = GameObject.Find("PowerUps");
        Vector2Int initialPowerupsPositions = c.activeTile.gridLocation;
        Vector3 initialWorldPosition = MapManager.Instance.GetWorldPositionFromTileLocation(initialPowerupsPositions);
        bomb = Instantiate(bombitaa, initialWorldPosition, Quaternion.identity).GetComponent<bomb>();
        
        OverlayTile initialTile = MapManager.Instance.GetTileFromTileLocation(initialPowerupsPositions);
        bomb.transform.SetParent(powerUpsContainer.transform);

        if (initialTile != null)
        {
            PositionPowerupOnTile(bomb, initialTile);
        }
        else
        {
            Debug.LogError("Tile inicial no encontrado para la posicion: " + initialPowerupsPositions);
        }

        
    }
    public void CrearBomb(IAInfo c)
    {
        GameObject powerUpsContainer = GameObject.Find("PowerUps");
        Vector2Int initialPowerupsPositions = c.activeTileIA.gridLocation;
        Vector3 initialWorldPosition = MapManager.Instance.GetWorldPositionFromTileLocation(initialPowerupsPositions);
        bomb = Instantiate(bombitaa, initialWorldPosition, Quaternion.identity).GetComponent<bomb>();

        OverlayTile initialTile = MapManager.Instance.GetTileFromTileLocation(initialPowerupsPositions);
        bomb.transform.SetParent(powerUpsContainer.transform);

        if (initialTile != null)
        {
            PositionPowerupOnTile(bomb, initialTile);
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

    private void PositionPowerupOnTile(bomb bomb, OverlayTile tile)
    {
        bomb.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);  
        bomb.GetComponent<SpriteRenderer>().sortingOrder = 30;
        bomb.activeTileBomb = tile;
    }
}
