using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class BorrarTileAleatorio : MonoBehaviour
{
    public Tilemap tilemap; // Asigna el Tilemap desde el Editor de Unity
    public RuleTile ruleTile;
    public RuleTile ruleTileTower;
    private Torreta torretaScript;

    public Vector2Int randomCajaInt;
    public List<Vector2> ListaCajas = new List<Vector2>();
    private void Update()
    {
        if (publicVariables.createScene == 1)
        {
            publicVariables.createScene = 2;
            torretaScript = FindObjectOfType<Torreta>();
            StartCoroutine(GenerarObstaculos());
        }
    }

    private IEnumerator GenerarObstaculos()
    {
        for (int i = 0; i < 25; i++)
        {
            yield return new WaitForSeconds(0.2f);
            GenerarObstaculoAleatorio();
        }
        publicVariables.createScene = 3;

    }

    private void GenerarObstaculoAleatorio()
    {
        // Genera una posición aleatoria dentro del tamaño del Tilemap
        Vector2 randomTilePosition = new Vector2(
            Random.Range(-8, 8),
            Random.Range(-6, 3)
        );

        randomCajaInt = Vector2Int.RoundToInt(randomTilePosition);
        while (ListaCajas.Contains(randomCajaInt))
        {
            randomTilePosition = new Vector2(Random.Range(-8, 8), Random.Range(-6, 3));
            randomCajaInt = new Vector2Int((int)randomTilePosition.x, (int)randomTilePosition.y);
            Debug.Log("SE REINICIAN LAS CAJAS");

        }


        ListaCajas.Add(randomCajaInt);

        Vector3Int randomTilePositionv3 = new Vector3Int(Mathf.RoundToInt(randomTilePosition.x), Mathf.RoundToInt(randomTilePosition.y), 0);

        // Establece el RuleTile para el obstáculo aleatorio
        tilemap.SetTile(randomTilePositionv3, ruleTile);
        tilemap.RefreshTile(randomTilePositionv3);

        // Establece el RuleTile para la torreta en su posición
        Vector2Int torretaPosition = torretaScript.GetTorretaPosition();
        Vector3Int vectorTower = new Vector3Int(torretaPosition.x, torretaPosition.y, 0);
        tilemap.SetTile(vectorTower, ruleTileTower);
        tilemap.RefreshTile(vectorTower);
    }
}