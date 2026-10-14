namespace SparseMatrixApp;

/// <summary>
/// Represents a single non-zero element in a sparse matrix linked list.
/// Stored in row-major order: sorted primarily by Row, secondarily by Col.
/// </summary>
public class SparseMatrixNode
{
    /// <summary>
    /// Zero-based row index.
    /// </summary>
    public int Row { get; set; }

    /// <summary>
    /// Zero-based column index.
    /// </summary>
    public int Col { get; set; }

    /// <summary>
    /// Non-zero pixel intensity (e.g. 0-255 in digital X-ray radiography).
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Pointer / reference to the next non-zero node in the linked list.
    /// </summary>
    public SparseMatrixNode? Next { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SparseMatrixNode"/> class.
    /// </summary>
    public SparseMatrixNode(int row, int col, int value, SparseMatrixNode? next = null)
    {
        Row = row;
        Col = col;
        Value = value;
        Next = next;
    }

    /// <summary>
    /// Formats the node in the canonical format: (Row, Col, Value).
    /// </summary>
    public override string ToString() => $"[Row: {Row}, Col: {Col}, Val: {Value}]";

    /// <summary>
    /// Compact representation matching task specification: "Row Col Value".
    /// </summary>
    public string ToCompactString() => $"{Row} {Col} {Value}";
}
