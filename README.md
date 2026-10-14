# Sparse Matrix in X-Ray Image Processing (.NET 10)

A modular, high-performance C# .NET 10 console application illustrating the use of a **Singly Linked List** to represent and manipulate **Sparse Matrices** in digital radiography (X-ray image processing).

---

## 📋 Table of Contents
1. [Radiography & Medical Imaging Context](#-radiography--medical-imaging-context)
2. [Why Linked Lists for Sparse Matrices?](#-why-linked-lists-for-sparse-matrices)
3. [Sparse Matrix Node & Invariants](#-sparse-matrix-node--invariants)
4. [Task Walkthrough](#-task-walkthrough)
   - [Task 1: Create a Sparse Matrix as a Linked List](#task-1-create-a-sparse-matrix-as-a-linked-list)
   - [Task 2: Display Sparse Matrix](#task-2-display-sparse-matrix)
   - [Task 3: Add Two Sparse Matrices](#task-3-add-two-sparse-matrices)
   - [Task 4: Transpose a Sparse Matrix](#task-4-transpose-a-sparse-matrix)
5. [Memory & Sparsity Analysis](#-memory--sparsity-analysis)
6. [Project Structure & Architecture](#-project-structure--architecture)
7. [Running the Application](#-running-the-application)
8. [Automated Verification Test Suite](#-automated-verification-test-suite)

---

## 🩺 Radiography & Medical Imaging Context

In digital radiography (X-ray imaging), images are stored as grayscale matrices where:
- Each pixel intensity ranges from `0` (completely dark / radiolucent air / background) to `255` (dense cortical bone / bright white).
- A large portion of an X-ray image (often 85% to 98%), such as background air, empty space, or soft non-attenuating tissue, has a value of `0`.
- Only anatomical structures (bone structures, implants, or contrast-enhanced tissue) appear as non-zero pixel values.

### Example 4×4 X-Ray Image Segment
Consider the 4×4 image segment from the task specification:
```
         Col 0   Col 1   Col 2   Col 3
Row 0        0       0     120       0
Row 1        0     200       0       0
Row 2        0       0       0      90
Row 3        0       0       0       0
```
Only three pixel intensities are non-zero:
- `(0, 2) = 120` (Cartilage / soft tissue density)
- `(1, 1) = 200` (Cortical bone density)
- `(2, 3) = 90`  (Spongy bone / edge)

Storing all 13 zeros wastes a significant amount of memory, especially in full-resolution scans ($1024 \times 1024$ or $2048 \times 2048$).

---

## 💡 Why Linked Lists for Sparse Matrices?

1. **Memory Efficiency**: Only non-zero elements are allocated. Zeros consume zero memory.
2. **Fast Matrix Addition ($O(N_1 + N_2)$)**: Adding two sparse matrices is identical to merging two sorted linked lists. In multi-exposure radiography, addition blends overlapping exposures to boost anatomical contrast.
3. **Linear Matrix Transposition ($O(\text{Cols} + N)$)**: Swapping axes for image reorientation and alignment runs in linear time using column-bucket linking.
4. **Dynamic Resizability**: Non-zero elements can be inserted, modified, or removed dynamically without reallocating contiguous 2D memory blocks.

---

## 🧱 Sparse Matrix Node & Invariants

### 1. Node Structure
Each non-zero element of the matrix is stored as a node containing:
```
┌───────────────────────────────────────────────┐
│              SparseMatrixNode                 │
├─────────┬─────────┬──────────────┬────────────┤
│   Row   │   Col   │    Value     │    Next    │
│  (int)  │  (int)  │ (int: 0-255) │ (Node ref) │
└─────────┴─────────┴──────────────┴────────────┘
```

- **Row**: Zero-based row index.
- **Col**: Zero-based column index.
- **Value**: Non-zero pixel intensity ($1 \le \text{Value} \le 255$).
- **Next**: Pointer/reference to the next node in the linked list (`null` at the tail).

### 2. Row-Major Ordering Invariant
All nodes in the linked list are strictly maintained in **row-major order**:
$$\text{Node } A \text{ precedes Node } B \iff (A.\text{Row} < B.\text{Row}) \lor (A.\text{Row} = B.\text{Row} \land A.\text{Col} < B.\text{Col})$$

This invariant guarantees that traversal, lookups, addition, and display algorithms operate deterministically in linear time.

The example 4×4 matrix is stored as the linked list:
```
[0, 2, 120] ──> [1, 1, 200] ──> [2, 3, 90] ──> null
```
Matching the task specification:
```
0 2 120 -> 1 1 200 -> 2 3 90 -> null
```

---

## 🔍 Task Walkthrough

### Task 1: Create a Sparse Matrix as a Linked List
- **Class**: `SparseMatrix(int rows, int cols)`
- **Ordered Insertion**: `Insert(int row, int col, int value)`
  - If `value == 0`: If a node exists at `(row, col)`, it is removed from the list.
  - If node exists at `(row, col)`: Updates its value in place.
  - Otherwise: Inserts a new `SparseMatrixNode` at the correct row-major sorted position in $O(N)$ worst-case time without needing a full sort.
- **Dense Array Factory**: `SparseMatrix.FromDense(int[,] array)` converts standard 2D arrays to a sparse linked list in $O(R \times C)$.

### Task 2: Display Sparse Matrix
The application renders matrices in multiple complementary formats:
1. **Compact Task Specification Format**:
   ```
   0 2 120 -> 1 1 200 -> 2 3 90 -> null
   ```
2. **Detailed Linked List Node Format**:
   ```
   [Row: 0, Col: 2, Val: 120] -> [Row: 1, Col: 1, Val: 200] -> [Row: 2, Col: 3, Val: 90] -> null
   ```
3. **Aligned 2D Grid with ANSI Highlight**:
   ```
            0    1    2    3
        --------------------
     0 |    0    0  120    0
     1 |    0  200    0    0
     2 |    0    0    0   90
     3 |    0    0    0    0
   ```

### Task 3: Add Two Sparse Matrices
In medical image processing, combining multiple exposures blends overlapping regions:
- **Algorithm**: Two-pointer linear merge of sorted linked lists.
- **Time Complexity**: $O(N_1 + N_2)$ where $N_1, N_2$ are the non-zero counts.
- **Merge Logic**:
  - Compare `(p1.Row, p1.Col)` and `(p2.Row, p2.Col)`:
    - If `p1` is before `p2`: append `p1` to result list, advance `p1`.
    - If `p2` is before `p1`: append `p2` to result list, advance `p2`.
    - If coordinates match: `sum = p1.Value + p2.Value`. If `sum != 0`, append node with `sum`, advance both `p1` and `p2`. If `sum == 0`, cancel and drop the node.
- **Operator Overload**: `matA + matB` or `matA.Add(matB)`.

#### Example Dual-Exposure Addition:
- **Exposure A (Bone)**: `(0,2,120) -> (1,1,200) -> (2,3,90)`
- **Exposure B (Contrast)**: `(1,1,45) -> (1,3,85) -> (2,3,60) -> (3,0,110)`
- **Result (A + B)**:
  - `(0, 2)`: `120`
  - `(1, 1)`: `200 + 45 = 245` (Enhanced bone structure)
  - `(1, 3)`: `85` (New contrast feature)
  - `(2, 3)`: `90 + 60 = 150` (Enhanced joint structure)
  - `(3, 0)`: `110` (New anatomical marker)
  - Result List: `0 2 120 -> 1 1 245 -> 1 3 85 -> 2 3 150 -> 3 0 110 -> null`

### Task 4: Transpose a Sparse Matrix
Transposition swaps rows and columns `(Row, Col) -> (Col, Row)`. In radiography, this aligns axes for multi-planar reconstruction or image orientation comparison.

- **Fast Transpose Algorithm**:
  Instead of an expensive $O(N \log N)$ sort or $O(N^2)$ repeated insertion, the implementation uses **Linear Column-Bucket Linking**:
  1. Create an array of bucket heads/tails of length `Cols`.
  2. Traverse original list: a node `(r, c, v)` becomes `(c, r, v)` and is appended to bucket `c`.
     - Because original nodes are in ascending row order, within each bucket $c$, the new column indices $r$ are already strictly ascending!
  3. Concatenate non-empty buckets from $c = 0$ to $\text{Cols} - 1$.
- **Time Complexity**: $O(\text{Cols} + N)$ — completely linear!
- **Ordering**: The transposed linked list is guaranteed to be in strict row-major order with zero comparisons.

#### Example Transposition:
- Original: `0 2 120 -> 1 1 200 -> 2 3 90 -> null`
- Node `(0, 2, 120)` $\to$ `(2, 0, 120)`
- Node `(1, 1, 200)` $\to$ `(1, 1, 200)`
- Node `(2, 3, 90)`  $\to$ `(3, 2, 90)`
- Transposed Result: `1 1 200 -> 2 0 120 -> 3 2 90 -> null`

---

## 📊 Memory & Sparsity Analysis

### 64-bit .NET CLR Memory Comparison
- **Dense 2D Array**:
  $$\text{Memory} = 40 \text{ bytes (object header \& bounds)} + (\text{Rows} \times \text{Cols} \times 4 \text{ bytes})$$
- **Sparse Linked List**:
  $$\text{Memory} = 56 \text{ bytes (matrix object)} + (N \times 40 \text{ bytes per node})$$
  *(Node: 16B object header + 4B Row + 4B Col + 4B Value + 4B padding + 8B Next pointer = 40 bytes)*

### Mathematical Break-Even Threshold
$$\text{Memory}_{\text{sparse}} < \text{Memory}_{\text{dense}} \iff N \times 40 < \text{TotalElements} \times 4 \iff \frac{N}{\text{TotalElements}} < 0.10$$
Therefore, when **sparsity exceeds 90%** (i.e. background is $\ge 90\%$ empty space/air), the linked list uses strictly less memory than the dense array.

### High-Resolution Radiography Scenario
For a $1024 \times 1024$ **Digital Subtraction Angiography (DSA)** scan:
- **Total Pixels**: $1,048,576$
- **Background Air/Soft Space**: $96.0\%$ sparsity
- **Dense 2D Array Memory**: $4.00 \text{ MB}$
- **Sparse Linked List Memory**: $1.60 \text{ MB}$
- **Net Memory Savings**: **$2.51 \text{ MB}$ ($60.0\%$ reduction)**

---

## 📂 Project Structure & Architecture

```
sparse-matrix/
├── .gitignore               # Standard .NET gitignore (bin, obj, IDE caches, macOS files)
├── README.md                # Comprehensive documentation
├── sparse-matrix.csproj     # .NET 10 console project definition
├── SparseMatrixNode.cs      # Linked list node (Row, Col, Value, Next)
├── SparseMatrix.cs          # Core sparse matrix data structure & algorithms
├── XRayScenarios.cs         # Radiography samples (4x4 prompt, dual exposure, 12x12 chest)
├── SparseMatrixTests.cs     # Automated verification test suite (10 test cases)
└── Program.cs               # Interactive CLI menu & guided demonstration
```

---

## 🚀 Running the Application

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/)

### 1. Interactive Console UI
Run the application to access the interactive menu:
```bash
dotnet run
```
Menu options:
```
  1. Run Full End-to-End Walkthrough (Tasks 1, 2, 3, 4 + Memory Analysis)
  2. Task 1 & 2: Create & Display Sparse Matrix (Prompt's 4x4 Example)
  3. Task 3: Add Two Sparse Matrices (Dual-Exposure X-Ray Blending)
  4. Task 4: Transpose a Sparse Matrix (Image Alignment / Rotation)
  5. Clinical Chest X-Ray Scenario & Memory Savings Benchmark
  6. Create & Inspect Custom Sparse Matrix (Interactive)
  7. Run Automated Verification Test Suite
  8. Exit
```

### 2. Run Direct Demonstration
```bash
dotnet run -- --demo
```

### 3. Run Automated Tests
```bash
dotnet run -- --test
```

---

## 🧪 Automated Verification Test Suite

The built-in verification suite validates 10 test cases:

| Test Case | Description | Result |
| :--- | :--- | :---: |
| **Task 1 & 2** | Create 4×4 X-ray Segment & Verify Linked List Order (`0 2 120 -> 1 1 200 -> 2 3 90 -> null`) | **PASS** |
| **Fidelity** | Dense Array Round-Trip (`FromDense` $\leftrightarrow$ `ToDense`) | **PASS** |
| **Ordering** | Arbitrary-Order Insertions Maintain Strict Row-Major Order | **PASS** |
| **Node Updates** | Update Existing Element & Removal via 0-Value Insertion | **PASS** |
| **Task 3** | Add Two Sparse Matrices (Merging Disjoint & Overlapping Nodes) | **PASS** |
| **Cancellation** | Sparse Matrix Addition: Opposing Sums Cancel to 0 | **PASS** |
| **Validation** | Matrix Addition Dimension Mismatch Throws `InvalidOperationException` | **PASS** |
| **Task 4** | Transpose 4×4 X-ray Matrix (`1 1 200 -> 2 0 120 -> 3 2 90 -> null`) | **PASS** |
| **Invariance** | Transpose Rectangular Matrix (3×5 $\to$ 5×3) & Double-Transpose $(M^T)^T = M$ | **PASS** |
| **Edge Cases** | Empty Matrix Operations (Zero Elements, Null Lists) | **PASS** |

**Verification Score: 10 / 10 tests passed (100% success rate)**.
