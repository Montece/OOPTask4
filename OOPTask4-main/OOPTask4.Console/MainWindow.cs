using Terminal.Gui;

namespace OOPTask4.Console;

public sealed class MainWindow : Window
{
    private readonly ListView _suppliersAccessoriesView;
    private readonly ListView _suppliersCarcasesView;
    private readonly ListView _suppliersEnginesView;
    private readonly ListView _workersView;
    private readonly ListView _warehouseCarcaseView;
    private readonly ListView _warehouseAccessoryView;
    private readonly ListView _warehouseEngineView;
    private readonly ListView _warehouseCarView;
    private readonly Label _carsSoldView;

    public MainWindow()
    {
        Title = $"OOPTask4 Console ({Application.QuitKey} to quit)";
        
        ArgumentNullException.ThrowIfNull(MainWindowController.CarBusiness);

        _suppliersAccessoriesView = new(MainWindowController.CarBusiness.SuppliersAccessories.GetTickables().ToList())
        {
            X = 0,
            Y = 0,
            Width = 25,
            Height = 5,
        };
        
        _suppliersCarcasesView = new(MainWindowController.CarBusiness.SuppliersCarcase.GetTickables().ToList())
        {
            X = Pos.Right(_suppliersAccessoriesView) + 1,
            Y = 0,
            Width = 25,
            Height = 5,
        };
        
        _suppliersEnginesView = new(MainWindowController.CarBusiness.SuppliersEngine.GetTickables().ToList())
        {
            X = Pos.Right(_suppliersCarcasesView) + 1,
            Y = 0,
            Width = 25,
            Height = 5,
        };

        _workersView = new(MainWindowController.CarBusiness.Workers.GetTickables().ToList())
        {
            X = 0,
            Y = Pos.Bottom(_suppliersAccessoriesView) + 1,
            Width = 25,
            Height = 5,
        };

        _warehouseCarcaseView = new(MainWindowController.CarBusiness.WarehouseCarcase.GetProducts().ToList())
        {
            X = 0,
            Y = Pos.Bottom(_workersView) + 1,
            Width = 25,
            Height = 5,
        };
        
        _warehouseAccessoryView = new(MainWindowController.CarBusiness.WarehouseAccessory.GetProducts().ToList())
        {
            X = Pos.Right(_warehouseCarcaseView) + 1,
            Y = Pos.Bottom(_workersView) + 1,
            Width = 25,
            Height = 5,
        };
        
        _warehouseEngineView = new(MainWindowController.CarBusiness.WarehouseEngine.GetProducts().ToList())
        {
            X = Pos.Right(_warehouseAccessoryView) + 1,
            Y = Pos.Bottom(_workersView) + 1,
            Width = 25,
            Height = 5,
        };

        _warehouseCarView = new(MainWindowController.CarBusiness.WarehouseCar.GetProducts().ToList())
        {
            X = Pos.Right(_warehouseEngineView) + 1,
            Y = Pos.Bottom(_workersView) + 1,
            Width = 25,
            Height = 5,
        };

        _carsSoldView = new()
        {
            X = 0,
            Y = Pos.Bottom(_warehouseCarcaseView) + 1,
            Width = 25,
            Height = 5,
        };

        Add(_suppliersAccessoriesView, _suppliersCarcasesView, _suppliersEnginesView);
        Add(_workersView);
        Add(_warehouseCarcaseView, _warehouseAccessoryView, _warehouseAccessoryView, _warehouseEngineView, _warehouseCarView);
        Add(_carsSoldView);

        Task.Run(RefreshData);
    }

    private void RefreshData()
    {
        ArgumentNullException.ThrowIfNull(MainWindowController.CarBusiness);

        while (MainWindowController.CarBusiness.BusinessIsRunning)
        {
            Application.MainLoop.Invoke(() =>
            {
                _suppliersAccessoriesView.SetSource(null);
                _suppliersAccessoriesView.SetSource(MainWindowController.CarBusiness.SuppliersAccessories.GetTickables().ToList());
                _suppliersCarcasesView.SetSource(null);
                _suppliersCarcasesView.SetSource(MainWindowController.CarBusiness.SuppliersCarcase.GetTickables().ToList());
                _suppliersEnginesView.SetSource(null);
                _suppliersEnginesView.SetSource(MainWindowController.CarBusiness.SuppliersEngine.GetTickables().ToList());
                _workersView.SetSource(null);
                _workersView.SetSource(MainWindowController.CarBusiness.Workers.GetTickables().ToList());
                _warehouseCarcaseView.SetSource(null);
                _warehouseCarcaseView.SetSource(MainWindowController.CarBusiness.WarehouseCarcase.GetProducts().ToList());
                _warehouseAccessoryView.SetSource(null);
                _warehouseAccessoryView.SetSource(MainWindowController.CarBusiness.WarehouseAccessory.GetProducts().ToList());
                _warehouseEngineView.SetSource(null);
                _warehouseEngineView.SetSource(MainWindowController.CarBusiness.WarehouseEngine.GetProducts().ToList());
                _warehouseCarView.SetSource(null);
                _warehouseCarView.SetSource(MainWindowController.CarBusiness.WarehouseCar.GetProducts().ToList());
                _carsSoldView.Text = $"Cars sold: {MainWindowController.CarBusiness.Dealers.GetTickables().Sum(d => d.CarsSoldCount)}";
            });

            Task.Delay(MainWindowController.CarBusiness.CarDealDelay).Wait();
        }
    }
}