namespace SparseMatrixApp;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Non-interactive mode for CI/test automation
        if (args.Length > 0 && args[0].Equals("--test", StringComparison.OrdinalIgnoreCase))
        {
            SparseMatrixTests.RunAllTests();
            return;
        }

        if (args.Length > 0 && args[0].Equals("--demo", StringComparison.OrdinalIgnoreCase))
        {
            RunFullDemonstration();
            return;
        }

        // Default run: display banner, execute the full guided walkthrough, then show interactive menu
        PrintBanner();
        RunFullDemonstration();

        // Interactive menu loop
        bool exit = false;
        while (!exit)
        {
            PrintMenu();
            Console.Write("\nSelect an option (1-8): ");
            string? choice = Console.ReadLine()?.Trim();

            Console.WriteLine();
            switch (choice)
            {
                case "1":
                    RunFullDemonstration();
                    break;
                case "2":
                    DemoCreateAndDisplay();
                    break;
                case "3":
                    DemoMatrixAddition();
                    break;
                case "4":
                    DemoMatrixTranspose();
                    break;
                case "5":
                    DemoClinicalChestRadiography();
                    break;
                case "6":
                    InteractiveCustomMatrix();
                    break;
                case "7":
                    SparseMatrixTests.RunAllTests();
                    break;
                case "8":
                case "exit":
                case "q":
                    exit = true;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Exiting Sparse Matrix Radiography Application. Goodbye!");
                    Console.ResetColor();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid selection. Please choose an option between 1 and 8.");
                    Console.ResetColor();
                    break;
            }
        }
    }

    private static void PrintBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════════════════════╗
║       SPARSE MATRIX IN DIGITAL RADIOGRAPHY (X-RAY) PROCESSING             ║
║                 C# .NET 10 Linked-List Implementation                     ║
╚═══════════════════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine("In digital X-ray imaging, images are grayscale matrices where pixel intensities");
        Console.WriteLine("range from 0 (black background/air) to 255 (dense white bone/structures).");
        Console.WriteLine("Because the background is mostly zero, Linked Lists provide massive memory");
        Console.WriteLine("savings and enable rapid matrix operations (Addition & Transposition).\n");
    }

    private static void PrintMenu()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("\n=============================== MAIN MENU ===============================");
        Console.ResetColor();
        Console.WriteLine("  1. Run Full End-to-End Walkthrough (Tasks 1, 2, 3, 4 + Memory Analysis)");
        Console.WriteLine("  2. Task 1 & 2: Create & Display Sparse Matrix (Prompt's 4x4 Example)");
        Console.WriteLine("  3. Task 3: Add Two Sparse Matrices (Dual-Exposure X-Ray Blending)");
        Console.WriteLine("  4. Task 4: Transpose a Sparse Matrix (Image Alignment / Rotation)");
        Console.WriteLine("  5. Clinical Chest X-Ray Scenario & Memory Savings Benchmark");
        Console.WriteLine("  6. Create & Inspect Custom Sparse Matrix (Interactive)");
        Console.WriteLine("  7. Run Automated Verification Test Suite");
        Console.WriteLine("  8. Exit");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("=========================================================================");
        Console.ResetColor();
    }

    public static void RunFullDemonstration()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(">>>>>>>>>>>>>>>> STARTING COMPREHENSIVE RADIOGRAPHY WALKTHROUGH <<<<<<<<<<<<<<<<");
        Console.ResetColor();

        // Task 1 & 2
        DemoCreateAndDisplay();

        // Task 3
        DemoMatrixAddition();

        // Task 4
        DemoMatrixTranspose();

        // Memory Benchmark
        DemoClinicalChestRadiography();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n>>>>>>>>>>>>>>>> COMPREHENSIVE WALKTHROUGH COMPLETE <<<<<<<<<<<<<<<<\n");
        Console.ResetColor();
    }

    private static void DemoCreateAndDisplay()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n==========================================================================");
        Console.WriteLine("  TASKS 1 & 2: CREATE & DISPLAY SPARSE MATRIX AS A LINKED LIST");
        Console.WriteLine("==========================================================================");
        Console.ResetColor();

        Console.WriteLine("Creating the exact 4x4 X-ray image segment specified in the problem:");
        Console.WriteLine("  - Pixel (0, 2) = 120 (Soft tissue / cartilage density)");
        Console.WriteLine("  - Pixel (1, 1) = 200 (Cortical bone density)");
        Console.WriteLine("  - Pixel (2, 3) = 90  (Spongy bone / edge)");
        Console.WriteLine("  - All remaining 13 pixels are 0 (Dark background)\n");

        var xRaySegment = XRayScenarios.CreateTaskExampleSegment();

        // Display Full 2D Grid
        xRaySegment.DisplayMatrix("Original 4x4 X-Ray Image Segment");

        // Display Linked List Format
        xRaySegment.DisplayList("Sparse Matrix Linked List");

        // Memory Analysis
        xRaySegment.DisplayMemoryAnalysis();
    }

    private static void DemoMatrixAddition()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n==========================================================================");
        Console.WriteLine("  TASK 3: ADD TWO SPARSE MATRICES (DUAL-EXPOSURE BLENDING)");
        Console.WriteLine("==========================================================================");
        Console.ResetColor();

        Console.WriteLine("Context: When combining multiple X-ray exposures or enhancing images,");
        Console.WriteLine("matrix addition blends overlapping regions to increase diagnostic contrast.\n");

        var exposureA = XRayScenarios.CreateTaskExampleSegment();
        var exposureB = XRayScenarios.CreateSecondaryExposureSegment();

        exposureA.DisplayMatrix("Exposure A (Primary Bone Scan - 4x4)");
        Console.WriteLine($"Linked List A: {exposureA.ToTaskLinkedString()}");

        exposureB.DisplayMatrix("Exposure B (Secondary Contrast Scan - 4x4)");
        Console.WriteLine($"Linked List B: {exposureB.ToTaskLinkedString()}");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nPerforming element-wise addition using Two-Pointer Linear Merge (O(N1 + N2))...");
        Console.ResetColor();

        var blendedExposure = exposureA + exposureB;

        blendedExposure.DisplayMatrix("Blended Result: Exposure A + Exposure B (4x4)");
        blendedExposure.DisplayList("Blended Result");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nObservations on Overlapping Nodes:");
        Console.WriteLine($"  - Coordinate (1, 1): Exposure A (200) + Exposure B (45) = {blendedExposure.Get(1, 1)} (Enhanced bone detail)");
        Console.WriteLine($"  - Coordinate (2, 3): Exposure A (90)  + Exposure B (60) = {blendedExposure.Get(2, 3)} (Enhanced joint detail)");
        Console.WriteLine($"  - Coordinate (1, 3): Exposure A (0)   + Exposure B (85) = {blendedExposure.Get(1, 3)} (New contrast feature)");
        Console.WriteLine($"  - Coordinate (3, 0): Exposure A (0)   + Exposure B (110)= {blendedExposure.Get(3, 0)} (New anatomical marker)");
        Console.ResetColor();
    }

    private static void DemoMatrixTranspose()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n==========================================================================");
        Console.WriteLine("  TASK 4: TRANSPOSE A SPARSE MATRIX (IMAGE ALIGNMENT / ROTATION)");
        Console.WriteLine("==========================================================================");
        Console.ResetColor();

        Console.WriteLine("Context: Matrix transpose swaps rows and columns (Row, Col) -> (Col, Row).");
        Console.WriteLine("In medical radiography, transposition is used for image reorientation,");
        Console.WriteLine("axis swapping, and multi-planar reconstruction alignment.\n");

        var original = XRayScenarios.CreateTaskExampleSegment();

        original.DisplayMatrix("Original Matrix (4x4)");
        Console.WriteLine($"Original Linked List:    {original.ToTaskLinkedString()}");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nTransposing using Linear Column-Bucket Linking (O(Cols + N))...");
        Console.ResetColor();

        var transposed = original.Transpose();

        transposed.DisplayMatrix("Transposed Matrix (4x4)");
        Console.WriteLine($"Transposed Linked List:  {transposed.ToTaskLinkedString()}");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nCoordinate Mapping Comparison:");
        Console.WriteLine("  Original Node (0, 2, 120)  --->  Transposed Node (2, 0, 120)");
        Console.WriteLine("  Original Node (1, 1, 200)  --->  Transposed Node (1, 1, 200)");
        Console.WriteLine("  Original Node (2, 3, 90)   --->  Transposed Node (3, 2, 90)");
        Console.ResetColor();
        Console.WriteLine("\nNotice that the transposed linked list automatically remains in strict");
        Console.WriteLine("row-major order: (1, 1, 200) -> (2, 0, 120) -> (3, 2, 90) -> null");
    }

    private static void DemoClinicalChestRadiography()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n==========================================================================");
        Console.WriteLine("  CLINICAL CHEST RADIOGRAPHY & REALISTIC MEMORY SAVINGS BENCHMARK");
        Console.WriteLine("==========================================================================");
        Console.ResetColor();

        Console.WriteLine("Simulating a 12x12 chest radiograph cross-section with spinal vertebrae");
        Console.WriteLine("and bilateral rib cage structures, surrounded by radiolucent (air) lungs.\n");

        var chestXray = XRayScenarios.CreateChestRadiographySection();

        chestXray.DisplayMatrix("12x12 Chest Radiograph (Spine & Ribs)");
        chestXray.DisplayList("12x12 Chest Radiograph");
        chestXray.DisplayMemoryAnalysis();

        // Transpose the chest radiograph
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nTransposing the 12x12 Chest Radiograph (Horizontal <-> Vertical alignment):");
        Console.ResetColor();

        var transposedChest = chestXray.Transpose();
        transposedChest.DisplayMatrix("Transposed 12x12 Chest Radiograph");
        Console.WriteLine($"Transposed Linked List has {transposedChest.NonZeroCount} non-zero nodes.");

        // Extrapolate to high-sparsity medical imaging (e.g., Digital Subtraction Angiography, Fluoroscopy, or high-res 1024x1024 / 2048x2048)
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n--- Mathematical Sparsity Threshold & High-Resolution Radiography ---");
        Console.ResetColor();
        Console.WriteLine("  Break-even analysis (64-bit runtime):");
        Console.WriteLine("    Dense matrix cell:   4 bytes (sizeof(int))");
        Console.WriteLine("    Linked list node:    ~40 bytes (Row, Col, Value, Next pointer, Object Header)");
        Console.WriteLine("    Break-even sparsity: 1 - (4 / 40) = 90.0% sparsity.");
        Console.WriteLine("    When background air/empty space exceeds 90%, the linked list saves massive memory!\n");

        int fullPixels = 1024 * 1024;
        long dense1024Bytes = (long)fullPixels * sizeof(int);
        // In Digital Subtraction Angiography (DSA) or targeted bone scans, non-zero vascular contrast is ~3-5%
        double contrastSparsity = 0.96; // 96% background
        int nonZeroEstimate = (int)(fullPixels * (1.0 - contrastSparsity));
        long sparse1024Bytes = 56 + ((long)nonZeroEstimate * 40);

        Console.WriteLine($"  Scenario: High-Res 1024x1024 Digital Subtraction Angiography (DSA)");
        Console.WriteLine($"  Resolution:                  1024 x 1024 ({fullPixels:N0} total pixels)");
        Console.WriteLine($"  Sparsity Level:              {contrastSparsity * 100:F1}% (Vascular contrast only 4% of field)");
        Console.WriteLine($"  Dense 2D Array Memory:       {dense1024Bytes:N0} bytes ({dense1024Bytes / 1024.0 / 1024.0:F2} MB)");
        Console.WriteLine($"  Sparse Linked List Memory:   {sparse1024Bytes:N0} bytes ({sparse1024Bytes / 1024.0 / 1024.0:F2} MB)");
        long savedBytes = dense1024Bytes - sparse1024Bytes;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  Memory Saved:                {savedBytes:N0} bytes ({((double)savedBytes / dense1024Bytes) * 100:F1}% reduction)");
        Console.ResetColor();
    }

    private static void InteractiveCustomMatrix()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n==========================================================================");
        Console.WriteLine("  INTERACTIVE CUSTOM SPARSE MATRIX BUILDER");
        Console.WriteLine("==========================================================================");
        Console.ResetColor();

        int rows = PromptInt("Enter number of rows (e.g. 4): ", 1, 50);
        int cols = PromptInt("Enter number of columns (e.g. 4): ", 1, 50);

        var matrix = new SparseMatrix(rows, cols);

        Console.WriteLine($"\nCreated empty {rows}x{cols} sparse matrix.");
        Console.WriteLine("Add non-zero elements (enter negative row to stop):");

        while (true)
        {
            Console.Write("\nEnter row index (-1 to finish): ");
            if (!int.TryParse(Console.ReadLine(), out int r) || r < 0)
                break;

            if (r >= rows)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Row must be between 0 and {rows - 1}.");
                Console.ResetColor();
                continue;
            }

            int c = PromptInt($"Enter column index (0 to {cols - 1}): ", 0, cols - 1);
            int v = PromptInt("Enter pixel intensity (1 to 255): ", 1, 100000);

            matrix.Insert(r, c, v);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Inserted [{r}, {c}] = {v}. Current non-zero count: {matrix.NonZeroCount}");
            Console.ResetColor();
        }

        matrix.DisplayMatrix("Your Custom Matrix");
        matrix.DisplayList("Your Custom Matrix");
        matrix.DisplayMemoryAnalysis();

        Console.Write("\nWould you like to transpose this matrix? (y/n): ");
        string? transposeAns = Console.ReadLine()?.Trim().ToLowerInvariant();
        if (transposeAns == "y" || transposeAns == "yes")
        {
            var transposed = matrix.Transpose();
            transposed.DisplayMatrix("Transposed Custom Matrix");
            transposed.DisplayList("Transposed Custom Matrix");
        }
    }

    private static int PromptInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int val) && val >= min && val <= max)
            {
                return val;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid input. Please enter an integer between {min} and {max}.");
            Console.ResetColor();
        }
    }
}
