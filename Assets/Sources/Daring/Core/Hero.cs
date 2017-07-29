
public class Hero
{
    public FlashLight FlashLight { get; private set; }

    public Hero()
    {
        FlashLight = new FlashLight();
    }
}