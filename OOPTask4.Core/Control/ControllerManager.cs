namespace OOPTask4.Core.Control;

public sealed class ControllerManager
{
    private readonly CarBusiness _carBusiness;

    public ControllerManager(CarBusiness carBusiness)
    {
        ArgumentNullException.ThrowIfNull(carBusiness);

        _carBusiness = carBusiness;
    }

    public void DoRequestForSupply()
    {
        _carBusiness.SuppliersAccessories.DoWork();
        _carBusiness.SuppliersCarcase.DoWork();
        _carBusiness.SuppliersEngine.DoWork();

        _carBusiness.Workers.DoWork();
    }
}