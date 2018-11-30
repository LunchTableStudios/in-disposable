namespace Indisposable.Input
{
    using UnityEngine;

    // public struct TouchData
    // {
    //     public int touchCount;
    //     public Vector2[] positions;
    // }

    // public static class TouchUtilities
    // {

    //     public const float GESTURE_TIME_THRESHOLD = 0.18f;

    //     public static TouchData GetTouchData()
    //     {
    //         TouchData touchData = new TouchData();
    //         touchData.touchCount = 0;

    //         if( Input.touchCount > 0 )
    //         {
    //             touchData.touchCount = Input.touchCount;
    //             touchData.positions = new Vector2[ Input.touchCount ];
    //             for( int i = 0; i < Input.touchCount; i++ )
    //                 touchData.positions[i] = Input.touches[i].position;
    //         }
    //         else if( Input.GetMouseButton(0) )
    //         {
    //             touchData.touchCount = 1;
    //             touchData.positions = new Vector2[]{ Input.mousePosition };
    //         }

    //         return touchData;
    //     }
    // }
}