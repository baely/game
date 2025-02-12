using Godot;
using System;

public class Background : TileMap
{
    private Camera2D camera;
    private int tileIndex = 0; // The index of your tile in the tileset

    public override void _Ready()
    {
        camera = GetNode<Camera2D>("../Character/Camera2D"); // Adjust path as needed
    }

    public override void _Process(float delta)
    {
        UpdateVisibleTiles();
    }

    private void UpdateVisibleTiles()
    {
        // Convert camera view into tilemap coordinates
        Vector2 topLeft = WorldToMap(camera.GetCameraScreenCenter() - camera.GetViewport().Size * camera.Zoom / 2);
        Vector2 bottomRight = WorldToMap(camera.GetCameraScreenCenter() + camera.GetViewport().Size * camera.Zoom / 2);

        // Add some padding to prevent visible edges
        topLeft -= new Vector2(2, 2);
        bottomRight += new Vector2(2, 2);

        // Fill the visible area with tiles
        for (int x = (int)topLeft.x; x <= (int)bottomRight.x; x++)
        {
            for (int y = (int)topLeft.y; y <= (int)bottomRight.y; y++)
            {
                // Only set cell if it's not already set
                if (GetCell(x, y) == -1)
                {
                    SetCell(x, y, tileIndex);
                }
            }
        }
    }
}