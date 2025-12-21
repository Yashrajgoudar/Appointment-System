var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.APIGateway_API>("apigateway");


builder.AddProject<Projects.AuthService_API>("authservice");
builder.AddProject<Projects.BookingService_API>("bookingservice");
builder.AddProject<Projects.CalendarService_API>("calendarservice");
builder.AddProject<Projects.NotificationService_API>("notificationservice");
builder.AddProject<Projects.PaymentService_API>("paymentservice");


builder.Build().Run();
