using HeartRateDemo.Models;
using HeartRateDemo.Services;
using SystemTimer = System.Timers.Timer;

namespace HeartRateDemo;

public partial class MainPage : ContentPage
{
    private readonly HeartRateMonitorScanner _heartRateMonitorScanner;
    private readonly HeartRateMonitorClient _heartRateMonitorClient;

    private bool _alreadyLoaded = false;
    private SystemTimer _sensorDiscoveryWatchdog;

    public MainPage(HeartRateMonitorScanner heartRateMonitorScanner,
        HeartRateMonitorClient heartRateMonitorClient)
    {
        _heartRateMonitorScanner = heartRateMonitorScanner;
        _heartRateMonitorClient = heartRateMonitorClient;

        InitializeComponent();
        BindingContext = this;
    }

    private string _status = "BotNet is scanning for your heart rate monitor";

    public string Status
    {
        get
        {
            return _status;
        }
        set
        {
            _status = value;
        }
    }

    public static readonly BindableProperty StatusBindableProperty = BindableProperty.Create(nameof(Status), typeof(string), typeof(MainPage),
        propertyChanged: new BindableProperty.BindingPropertyChangedDelegate((_bindable, _old, _new) =>
        {
            _bindable.CoerceValue(StatusBindableProperty);
        }));

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_alreadyLoaded)
            return;

        _alreadyLoaded = true;

        HeartRateMonitor_Sensors_Subscribe();

        try
        {
            _heartRateMonitorScanner.Start();
        }
        catch (Exception ex)
        {
            // Add exception handling
        }
    }

    private void HeartRateMonitor_Sensors_Subscribe()
    {
        try
        {
            _heartRateMonitorClient.Connected += HeartRateMonitor_Sensors_Connected;
            _heartRateMonitorClient.Disconnected += HeartRateMonitor_Sensors_Disconnected;
            _heartRateMonitorClient.HeartRateUpdated += HeartRateMonitor_Sensors_HeartRateUpdated;

            _heartRateMonitorScanner.Sensors.CollectionChanged += HeartRateMonitor_Sensors_Changed;
        }
        catch (Exception ex)
        {
            // Add exception handling
        }
    }

    private void HeartRateMonitor_Sensors_HeartRateUpdated(object sender, HeartRateSensor sensor, uint heartRate)
    {
        if (heartRate > 0)
        {
            Status = $"Your heart rate is {heartRate} bpm";
        }
    }

    private void HeartRateMonitor_Sensors_Connected(object sender, HeartRateSensor sensor)
    {
        try
        {
            Status = $"BotNet is connected to {sensor.Name}";
        }
        catch
        {
            // Add exception handling
        }
    }

    private void HeartRateMonitor_Sensors_Disconnected(object sender, HeartRateSensor sensor)
    {
        try
        {
            Status = $"BotNet is disconnected";
        }
        catch
        {
            // Add exception handling
        }
    }

    private void HeartRateMonitor_Sensors_Changed(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        try
        {
            // Search for the nearest Rssi, after a device is detected, wait for x seconds for other devices, re-sort by RSSI
            // and then process the sensor for connection
            if (e != null &&
                e.NewItems != null &&
                _sensorDiscoveryWatchdog == null)
            {
                HeartRateMonitor_Sensors_Timer_Start();
                _heartRateMonitorScanner.Sensors.CollectionChanged -= HeartRateMonitor_Sensors_Changed;
            }
        }
        catch (Exception ex)
        {
            // Add exception handling
        }
    }

    private void HeartRateMonitor_Sensors_Timer_Start()
    {
        try
        {
            if (_sensorDiscoveryWatchdog != null)
            {
                _sensorDiscoveryWatchdog.Stop();
                _sensorDiscoveryWatchdog.Dispose();
                _sensorDiscoveryWatchdog = null;
            }

            _sensorDiscoveryWatchdog = new SystemTimer(TimeSpan.FromSeconds(2));
            _sensorDiscoveryWatchdog.AutoReset = false;
            _sensorDiscoveryWatchdog.Elapsed += HeartRateMonitor_Sensors_Timer_Elapsed;
            _sensorDiscoveryWatchdog.Start();
        }
        catch (Exception ex)
        {
            // Add exception handling
        }
    }

    private async void HeartRateMonitor_Sensors_Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            _heartRateMonitorScanner.Stop();
        }
        catch (Exception ex)
        {
            // Add exception handling
        }

        await Task.Run(async () =>
        {
            try
            {
                // Do we have enough items to sort
                if (_heartRateMonitorScanner.Sensors.Count == 0)
                    throw new Exception("Unable to continue - Not enough heart rate sensors were discovered. This is an internal issue.");

                // Create a sortable array of found sensors
                List<HeartRateSensor> found = new List<HeartRateSensor>(_heartRateMonitorScanner.Sensors);

                // Sort the sensors with a sorted array and Linq
                List<HeartRateSensor> sorted = found.OrderBy(a => a.Rssi)
                    .ToList();

                // Find the first sensor in the sorted listed
                HeartRateSensor sensor = sorted.First();

                // Dump details to the debug log for now
                System.Diagnostics.Debug.WriteLine($"Heart Rate Sensor Found: {sensor.Name} with UUID {sensor.Uuid}");

                // Prepare to connect to the heart rate montior
                Status = $"BotNet is connecting to {sensor.Name}";

                // Connect to the Bluetooth Heart Monitor
                _heartRateMonitorClient.Sensor = sensor;
                await _heartRateMonitorClient.Connect();

                // Allow subscriptions to handle status updates here...

                // Wait for further dispatch
                await Task.Delay(TimeSpan.FromMinutes(5));

                // Disconnect from the heart rate sensor
                _heartRateMonitorClient.Disconnect();
            }
            catch (Exception ex)
            {
                // Add exception handling
            }
        });
    }
}

