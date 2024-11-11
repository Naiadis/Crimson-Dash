using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [SerializeField] private GameObject groundPrefab;    // Add SerializeField to make it visible
    [SerializeField] private int groundPieceCount = 3;
    [SerializeField] private Transform player;

    private GameObject[] groundPieces;
    private int currentFirstPiece = 0;
    private float groundPieceLength;
    private float startX;

    void Start()
    {
        if (groundPrefab == null || player == null)
        {
            Debug.LogError("Please assign Ground Prefab and Player in the inspector!");
            return;
        }

        // Get the length of your ground piece from its sprite or collider
        groundPieceLength = groundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        startX = transform.position.x;

        // Create initial ground pieces
        groundPieces = new GameObject[groundPieceCount];
        for (int i = 0; i < groundPieceCount; i++)
        {
            groundPieces[i] = Instantiate(groundPrefab);
            groundPieces[i].transform.position = new Vector3(startX + (i * groundPieceLength), transform.position.y, 0);
            groundPieces[i].transform.parent = transform; // Parent to the manager for organization
        }
    }

    void Update()
    {
        if (player == null || groundPieces == null || groundPieces.Length == 0) return;

        // Check if player has passed the middle piece
        float playerX = player.position.x;
        int middlePiece = (currentFirstPiece + 1) % groundPieceCount;

        // If player passed middle piece, move the last piece to the front
        if (playerX > groundPieces[middlePiece].transform.position.x)
        {
            // Move the last piece to the front
            int lastPiece = (currentFirstPiece + groundPieceCount - 1) % groundPieceCount;
            groundPieces[lastPiece].transform.position = new Vector3(
                groundPieces[currentFirstPiece].transform.position.x + groundPieceLength,
                transform.position.y,
                0
            );
            currentFirstPiece = (currentFirstPiece + 1) % groundPieceCount;
        }
    }
}