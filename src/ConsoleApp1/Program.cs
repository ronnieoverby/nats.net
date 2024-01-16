/*
 * This demonstrates that despite `reconnectOnConnect: true` the connection never transitions away from `Closed`.
 *
 * Instruction manual:
 * 1) Local nats server ain't running
 * 2) Start this program and observe `{ State = CLOSED }` being repeatedly written to STDOUT
 * 3) Start local nats server
 * 4) State remains `Closed`; a connection is never established.
 *
 *
 * Notes:
 * I've tried various configurations of the `Options` class being passed to `CreateConnection()`, with no change in outcome.
 * I never see any of the default connection event handlers write anything to the console, as typically seen when
 * the connection state changes.
 * I tried stepping through the nats client code a few times, trying to understand what I may be doing wrong.
 * I don't believe reconnection is ever attempted. 
 * 
 */

using NATS.Client;

var exitSource = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArgs) =>
{
    exitSource.Cancel();
    eventArgs.Cancel = true;
};

var cf = new ConnectionFactory();
var connection = cf.CreateConnection(reconnectOnConnect: true);

while (!exitSource.IsCancellationRequested && connection.State is not ConnState.CONNECTED)
{
    Console.WriteLine(new { connection.State });
    Thread.Sleep(3333);
}

Console.WriteLine(new { connection.State });
exitSource.Token.WaitHandle.WaitOne();