using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using server.Entities;
using server.Exceptions;
using server.Models;

namespace server.Services;

public class MailService : IMailService
{
	private readonly AppDbContext _dbContext;

	public MailService(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	
	public async Task SendPasswordReset(ForgotPasswordDto dto, string token)
	{
		var user = _dbContext.Users.FirstOrDefault(x => x.Email == dto.Email);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika o podanym adresie e-mail");
		}

		var message = new MimeMessage();
		message.From.Add(new MailboxAddress("Dziennik dla alergików", "szyymiik@gmail.com"));
		message.To.Add(MailboxAddress.Parse(dto.Email));
		message.Subject = "Zresetuj hasło";
		message.Body = new TextPart(TextFormat.Html)
		{
			Text = @"<h2>Witaj "+user.Name+@"!</h2>
					<h3>Twój link do zresetowania hasła:<h3/>
					<p>http://localhost:3000/reset-password?uid=" + user.Id + "&token=" + token
		};

		var client = new SmtpClient();

		try
		{
			await client.ConnectAsync("smtp.gmail.com", 465, true);
			await client.AuthenticateAsync("szyymiik@gmail.com", "12345678");
			await client.SendAsync(message);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}
		finally
		{
			await client.DisconnectAsync(true);
			client.Dispose();
		}
	}
}