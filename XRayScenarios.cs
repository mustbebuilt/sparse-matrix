namespace SparseMatrixApp;

/// <summary>
/// Provides pre-configured X-ray image segments and scenarios demonstrating real-world
/// radiography use cases (dual exposure blending, alignment transpose, memory savings).
/// </summary>
public static class XRayScenarios
{
    /// <summary>
    /// Creates the exact 4x4 X-ray image segment specified in the prompt:
    ///   0   1   2   3
    /// 0 0   0 120   0
    /// 1 0 200   0   0
    /// 2 0   0   0  90
    /// 3 0   0   0   0
    /// Non-zeros: (0,2)=120, (1,1)=200, (2,3)=90
    /// Linked list: 0 2 120 -> 1 1 200 -> 2 3 90 -> null
    /// </summary>
    public static SparseMatrix CreateTaskExampleSegment()
    {
        var matrix = new SparseMatrix(4, 4);
        matrix.Insert(0, 2, 120);
        matrix.Insert(1, 1, 200);
        matrix.Insert(2, 3, 90);
        return matrix;
    }

    /// <summary>
    /// Creates a secondary 4x4 exposure segment representing a soft-tissue contrast enhancement.
    /// Used to demonstrate matrix addition (exposure blending).
    /// </summary>
    public static SparseMatrix CreateSecondaryExposureSegment()
    {
        var matrix = new SparseMatrix(4, 4);
        // Overlaps at (1,1) with +45 (bone edge enhancement)
        matrix.Insert(1, 1, 45);
        // New soft tissue feature at (1,3)
        matrix.Insert(1, 3, 85);
        // Overlaps at (2,3) with +60 (joint cartilage enhancement)
        matrix.Insert(2, 3, 60);
        // New anatomical marker at (3,0)
        matrix.Insert(3, 0, 110);
        return matrix;
    }

    /// <summary>
    /// Creates a 12x12 clinical chest X-ray spine and rib cross-section.
    /// Over 90% of the image is radiolucent air/background (black / 0),
    /// demonstrating significant memory savings using a linked list.
    /// </summary>
    public static SparseMatrix CreateChestRadiographySection()
    {
        var matrix = new SparseMatrix(12, 12);

        // Vertebral column down the center (Cols 5-6)
        for (int r = 1; r <= 10; r++)
        {
            matrix.Insert(r, 5, 210); // dense cortical bone
            matrix.Insert(r, 6, 195); // trabecular bone
        }

        // Rib structures arching outwards
        matrix.Insert(2, 2, 140);
        matrix.Insert(2, 3, 160);
        matrix.Insert(2, 8, 165);
        matrix.Insert(2, 9, 145);

        matrix.Insert(5, 1, 130);
        matrix.Insert(5, 2, 155);
        matrix.Insert(5, 9, 150);
        matrix.Insert(5, 10, 135);

        matrix.Insert(8, 2, 145);
        matrix.Insert(8, 3, 160);
        matrix.Insert(8, 8, 160);
        matrix.Insert(8, 9, 140);

        return matrix;
    }
}
