using System.Numerics;
using Veldrid;

namespace Hello_Shader;

public struct VertexPositionColor(Vector2 position, RgbaFloat color)
{
    public Vector2 Position = position;
    public RgbaFloat Color = color;
    public const uint SizeInBytes = 24;
}