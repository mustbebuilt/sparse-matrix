namespace SparseMatrixApp;

/// <summary>
/// Comprehensive verification test suite for SparseMatrix implementation.
/// </summary>
public static class SparseMatrixTests
{
    public static void RunAllTests()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n========================================================");
        Console.WriteLine("       AUTOMATED SPARSE MATRIX VERIFICATION SUITE       ");
        Console.WriteLine("========================================================");
        Console.ResetColor();

        int passed = 0;
        int total = 0;

        void Run(string testName, Action testAction)
        {
            total++;
            try
            {
                testAction();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  [PASS] {testName}");
                Console.ResetColor();
                passed++;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  [FAIL] {testName}: {ex.Message}");
                Console.ResetColor();
            }
        }

        Run("Task 1: Create 4x4 X-ray Segment & Verify Linked List Order", () =>
        {
            var matrix = XRayScenarios.CreateTaskExampleSegment();
            Assert(matrix.Rows == 4, "Rows should be 4");
            Assert(matrix.Cols == 4, "Cols should be 4");
            Assert(matrix.NonZeroCount == 3, "NonZeroCount should be 3");

            string expected = "0 2 120 -> 1 1 200 -> 2 3 90 -> null";
            string actual = matrix.ToTaskLinkedString();
            Assert(actual == expected, $"Expected: '{expected}', Got: '{actual}'");

            Assert(matrix.Get(0, 2) == 120, "Pixel (0,2) must be 120");
            Assert(matrix.Get(1, 1) == 200, "Pixel (1,1) must be 200");
            Assert(matrix.Get(2, 3) == 90, "Pixel (2,3) must be 90");
            Assert(matrix.Get(0, 0) == 0, "Pixel (0,0) must be 0");
            Assert(matrix.Get(3, 3) == 0, "Pixel (3,3) must be 0");
        });

        Run("Dense Array Round-Trip (FromDense & ToDense)", () =>
        {
            int[,] denseInput = new int[4, 4]
            {
                { 0,   0, 120,  0 },
                { 0, 200,   0,  0 },
                { 0,   0,   0, 90 },
                { 0,   0,   0,  0 }
            };

            var sparse = SparseMatrix.FromDense(denseInput);
            Assert(sparse.NonZeroCount == 3, "NonZeroCount should be 3");
            Assert(sparse.ToTaskLinkedString() == "0 2 120 -> 1 1 200 -> 2 3 90 -> null", "Linked string match");

            int[,] denseOutput = sparse.ToDense();
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    Assert(denseInput[r, c] == denseOutput[r, c], $"Mismatch at ({r},{c})");
                }
            }
        });

        Run("Arbitrary-Order Insertions Maintain Strict Row-Major Order", () =>
        {
            var matrix = new SparseMatrix(5, 5);
            // Insert in reverse / random order
            matrix.Insert(3, 2, 75);
            matrix.Insert(0, 4, 15);
            matrix.Insert(1, 1, 50);
            matrix.Insert(0, 1, 10);

            // Row-major order must be: (0,1,10) -> (0,4,15) -> (1,1,50) -> (3,2,75)
            string expected = "0 1 10 -> 0 4 15 -> 1 1 50 -> 3 2 75 -> null";
            Assert(matrix.ToTaskLinkedString() == expected, $"Order mismatch: {matrix.ToTaskLinkedString()}");
        });

        Run("Update Existing Element and Zero-Removal", () =>
        {
            var matrix = new SparseMatrix(3, 3);
            matrix.Insert(1, 1, 100);
            Assert(matrix.NonZeroCount == 1, "Count should be 1");

            // Overwrite
            matrix.Insert(1, 1, 180);
            Assert(matrix.NonZeroCount == 1, "Count should stay 1 after overwrite");
            Assert(matrix.Get(1, 1) == 180, "Value should be updated to 180");

            // Removing by inserting 0
            matrix.Insert(1, 1, 0);
            Assert(matrix.NonZeroCount == 0, "Count should be 0 after setting to 0");
            Assert(matrix.Head == null, "Head should be null");
            Assert(matrix.Get(1, 1) == 0, "Value should be 0");
        });

        Run("Task 3: Add Two Sparse Matrices (Merging Disjoint & Overlapping)", () =>
        {
            // Matrix A
            var matA = new SparseMatrix(4, 4);
            matA.Insert(0, 2, 120);
            matA.Insert(1, 1, 200);
            matA.Insert(2, 3, 90);

            // Matrix B
            var matB = new SparseMatrix(4, 4);
            matB.Insert(1, 1, 45);  // Overlaps with A (1,1) -> 200 + 45 = 245
            matB.Insert(1, 3, 85);  // Disjoint
            matB.Insert(2, 3, 60);  // Overlaps with A (2,3) -> 90 + 60 = 150
            matB.Insert(3, 0, 110); // Disjoint

            var matC = matA + matB;
            Assert(matC.Rows == 4 && matC.Cols == 4, "Result must be 4x4");
            Assert(matC.NonZeroCount == 5, "Result non-zero count should be 5");

            string expected = "0 2 120 -> 1 1 245 -> 1 3 85 -> 2 3 150 -> 3 0 110 -> null";
            Assert(matC.ToTaskLinkedString() == expected, $"Addition string mismatch: {matC.ToTaskLinkedString()}");

            Assert(matC.Get(0, 2) == 120, "(0,2) should be 120");
            Assert(matC.Get(1, 1) == 245, "(1,1) should be 245");
            Assert(matC.Get(1, 3) == 85, "(1,3) should be 85");
            Assert(matC.Get(2, 3) == 150, "(2,3) should be 150");
            Assert(matC.Get(3, 0) == 110, "(3,0) should be 110");
        });

        Run("Sparse Matrix Addition: Sum Cancellation to Zero", () =>
        {
            var matA = new SparseMatrix(2, 2);
            matA.Insert(0, 1, 50);

            var matB = new SparseMatrix(2, 2);
            matB.Insert(0, 1, -50);

            var matSum = matA + matB;
            Assert(matSum.NonZeroCount == 0, "Cancelling sum should result in 0 non-zero nodes");
            Assert(matSum.Head == null, "Head must be null");
        });

        Run("Sparse Matrix Addition Dimension Mismatch Throws Exception", () =>
        {
            var matA = new SparseMatrix(3, 4);
            var matB = new SparseMatrix(4, 3);
            bool caught = false;
            try
            {
                _ = matA + matB;
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }
            Assert(caught, "Should throw InvalidOperationException on dimension mismatch");
        });

        Run("Task 4: Transpose 4x4 X-ray Matrix", () =>
        {
            // Original: 0 2 120 -> 1 1 200 -> 2 3 90 -> null
            var matrix = XRayScenarios.CreateTaskExampleSegment();
            var transposed = matrix.Transpose();

            Assert(transposed.Rows == 4 && transposed.Cols == 4, "Dimensions should remain 4x4");
            Assert(transposed.NonZeroCount == 3, "NonZeroCount should remain 3");

            // (0,2,120) -> (2,0,120)
            // (1,1,200) -> (1,1,200)
            // (2,3,90)  -> (3,2,90)
            // In row-major order:
            // Row 1 Col 1 Val 200 -> Row 2 Col 0 Val 120 -> Row 3 Col 2 Val 90
            string expected = "1 1 200 -> 2 0 120 -> 3 2 90 -> null";
            string actual = transposed.ToTaskLinkedString();
            Assert(actual == expected, $"Expected: '{expected}', Got: '{actual}'");

            Assert(transposed.Get(1, 1) == 200, "Transposed (1,1) should be 200");
            Assert(transposed.Get(2, 0) == 120, "Transposed (2,0) should be 120");
            Assert(transposed.Get(3, 2) == 90, "Transposed (3,2) should be 90");
        });

        Run("Task 4: Transpose Rectangular Matrix (3x5 to 5x3) and Double-Transpose Invariant", () =>
        {
            var rect = new SparseMatrix(3, 5);
            rect.Insert(0, 4, 33);
            rect.Insert(1, 2, 55);
            rect.Insert(2, 0, 77);
            rect.Insert(2, 4, 99);

            var transposed = rect.Transpose();
            Assert(transposed.Rows == 5 && transposed.Cols == 3, "Transposed should be 5x3");

            // Double transpose must equal original
            var doubleTransposed = transposed.Transpose();
            Assert(doubleTransposed.Rows == 3 && doubleTransposed.Cols == 5, "Double transpose dimensions 3x5");
            Assert(doubleTransposed.ToTaskLinkedString() == rect.ToTaskLinkedString(), "Double transpose must equal original");
        });

        Run("Empty Matrix Operations (Zero Elements)", () =>
        {
            var empty = new SparseMatrix(3, 3);
            Assert(empty.NonZeroCount == 0, "Non-zero count must be 0");
            Assert(empty.Head == null, "Head must be null");
            Assert(empty.ToTaskLinkedString() == "null", "ToTaskLinkedString should be 'null'");

            var transposedEmpty = empty.Transpose();
            Assert(transposedEmpty.Head == null, "Transposed empty matrix must be null");

            var sumEmpty = empty + empty;
            Assert(sumEmpty.Head == null, "Sum of empty matrices must be null");
        });

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\nVerification Completed: {passed} / {total} tests passed ({(passed * 100.0 / total):F0}% success rate).");
        Console.ResetColor();
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
            throw new Exception($"Assertion failed: {message}");
    }
}
