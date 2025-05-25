// See https://aka.ms/new-console-template for more information

using System.Numerics;
using System.Text;
using Hello_Shader;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.SPIRV;
using Veldrid.StartupUtilities;

// Declare Vertices and indices for our Quad
VertexPositionColor[] quadVertices =
[
    new(new Vector2(-0.75f, 0.75f), RgbaFloat.Red),
    new(new Vector2(0.75f, 0.75f), RgbaFloat.Green),
    new(new Vector2(-0.75f, -0.75f), RgbaFloat.Blue),
    new(new Vector2(0.75f, -0.75f), RgbaFloat.Yellow)
];
ushort[] quadIndices = [0, 1, 2, 3];

//Other Variables
UInt64 ticks = 0;

//Create Veldrid Resources
var window = VeldridStartup.CreateWindow(
    new WindowCreateInfo{
    X = 100, Y = 100,
    WindowWidth = 800,
    WindowHeight = 600,
    WindowInitialState = WindowState.Normal,
    WindowTitle = "Hello Shader"});

var gd = VeldridStartup.CreateGraphicsDevice(
    window, 
    new GraphicsDeviceOptions {
    PreferDepthRangeZeroToOne = true,
    PreferStandardClipSpaceYDirection = true });

var factory = gd.ResourceFactory;

var commadList = factory.CreateCommandList();

var vertBuff = factory.CreateBuffer(new BufferDescription(4*VertexPositionColor.SizeInBytes, BufferUsage.VertexBuffer));
var idxBuff = factory.CreateBuffer(new BufferDescription(4*sizeof(ushort), BufferUsage.IndexBuffer));
gd.UpdateBuffer(vertBuff, 0, quadVertices);
gd.UpdateBuffer(idxBuff, 0, quadIndices);

var vertLayout = new VertexLayoutDescription(
    new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
    new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4)
);
const string vertexCode = 
    """
    #version 450

    layout(location = 0) in vec2 Position;
    layout(location = 1) in vec4 Color;

    layout(location = 0) out vec4 fsin_Color;

    void main()
    {
      gl_Position = vec4(Position, 0, 1);
      fsin_Color = Color;
    }
    """;

const string fragmentCode = 
    """
    #version 450

    layout(location = 0) in vec4 fsin_Color;
    layout(location = 0) out vec4 fsout_Color;

    void main()
    {
        fsout_Color = fsin_Color;
    }
    """;
var shaders = factory.CreateFromSpirv(
    new ShaderDescription(ShaderStages.Vertex, Encoding.UTF8.GetBytes(vertexCode), "main"),
    new ShaderDescription(ShaderStages.Fragment, Encoding.UTF8.GetBytes(fragmentCode), "main"));

var pipeline = factory.CreateGraphicsPipeline(    
    new GraphicsPipelineDescription{
    BlendState = BlendStateDescription.SingleOverrideBlend,
    DepthStencilState = new DepthStencilStateDescription(
        depthTestEnabled: true,
        depthWriteEnabled: true,
        comparisonKind: ComparisonKind.LessEqual),
    RasterizerState = new RasterizerStateDescription(
        cullMode: FaceCullMode.Back,
        fillMode: PolygonFillMode.Solid,
        frontFace: FrontFace.Clockwise,
        depthClipEnabled: true,
        scissorTestEnabled: false),
    PrimitiveTopology = PrimitiveTopology.TriangleStrip,
    ResourceLayouts = [],
    ShaderSet = new ShaderSetDescription(
        vertexLayouts: [vertLayout],
        shaders: shaders),
    Outputs = gd.SwapchainFramebuffer.OutputDescription, });

while (window.Exists)
{
    ticks++;
    Console.WriteLine($"Ticks: {ticks}");
    window.PumpEvents();
    Thread.Sleep(30);
}