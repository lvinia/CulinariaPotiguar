using UnityEngine;
using System.Collections.Generic;

public class RepetirLevel : MonoBehaviour
{
    public Transform player;
    public GameObject levelPiecePrefab;
    public float pieceLength = 85.17772f;
    public float triggerDistance = 40f;
    public int piecesAhead = 2;
    public GameObject firstPieceInScene;

    private List<GameObject> activePieces = new List<GameObject>();

    void Start()
    {
        activePieces.Add(firstPieceInScene);

        Vector3 spawnPosition = firstPieceInScene.transform.position + Vector3.right * pieceLength;

        for (int i = 1; i < piecesAhead; i++)
        {
            GameObject piece = Instantiate(levelPiecePrefab, spawnPosition, Quaternion.identity);
            activePieces.Add(piece);
            spawnPosition.x += pieceLength;
        }
    }

    void Update()
    {
        GameObject lastPiece = activePieces[activePieces.Count - 1];
        float distanceToEnd = (lastPiece.transform.position.x + pieceLength) - player.position.x;

        if (distanceToEnd < triggerDistance)
        {
            SpawnNextPiece();
        }

        
        GameObject firstPiece = activePieces[0];
        if (player.position.x - firstPiece.transform.position.x > pieceLength * 1.5f)
        {
            activePieces.RemoveAt(0);
            Destroy(firstPiece);
        }
    }

    void SpawnNextPiece()
    {
        GameObject lastPiece = activePieces[activePieces.Count - 1];
        Vector3 spawnPosition = lastPiece.transform.position + Vector3.right * pieceLength;

        GameObject newPiece = Instantiate(levelPiecePrefab, spawnPosition, Quaternion.identity);
        activePieces.Add(newPiece);
    }
}