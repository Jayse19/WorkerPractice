namespace shared;
public record Location(int row, int column);
public record EnlistRequest(string host, int port);
public record DoneWorking();
public record StartWorking(string code, string password);
public record State(string gameState);
public record Status(string workerName, State currentState, Location Destination);

