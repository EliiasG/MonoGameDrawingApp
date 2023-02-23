internal class Program
{
    private static void Main(string[] args)
    {
        using var game = new MonoGameDrawingApp.Game1();
        game.Run();
        //EarClippingGeometryTriangulator.Test();
    }
}