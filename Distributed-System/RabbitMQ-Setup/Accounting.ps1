#
# Accounting.ps1
#

param([String]$RabbitDllPath = "not specificed")
set-ExecutionPolicy Unrestricted

Write-Host "Rabbit DLL Path: "
Write-Host $RabbitDllPath -ForegroundColor Green

$absoluteRabbitDllPath = Resolve-Path $RabbitDllPath

Write-Host "Absolute Rabbit DLL Path: "
Write-Host $absoluteRabbitDllPath -ForegroundColor Green

[Reflection.Assembly]::LoadFile($absoluteRabbitDllPath)

$hostServer = "localhost"
$hostUsername = "guest"
$hostPassowrd = "guest"

Write-Host "Setting up RabbitMQ Connection Factory for host"
$factory = New-Object RabbitMQ.Client.ConnectionFactory
$hostName = [RabbitMQ.Client.ConnectionFactory].GetField("HostName");
$hostName.SetValue($factory, $hostServer);

$username = [RabbitMQ.Client.ConnectionFactory].GetField("UserName");
$username.SetValue($factory, $hostUsername);

$password = [RabbitMQ.Client.ConnectionFactory].GetField("UserName");
$password.SetValue($factory, $hostPassword);

$createConnectionMethod = [RabbitMQ.Client.ConnectionFactory].GetMethod("CreateConnection", [Type]::EmptyTypes)
$connection = $createConnectionMethod.Invoke($factory, "instance.public", $null, $null, $null);

Write-Host "Setting up RabbitMQ Model"
$model = $connection.CreateModel();

Write-Host "Creating Queue"
$model.QueueDeclare("Hello", $false, $false, $false, $null)

Write-Host "Setup Complete"