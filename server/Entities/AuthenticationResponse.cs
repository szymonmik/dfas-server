using server.Models;

namespace server.Entities;

public class AuthenticationResponse
{
	public UserDto User { get; set; }
	public string Token { get; set; }
	

	public AuthenticationResponse(UserDto userDto, string token)
	{
		User = userDto;
		Token = token;
	}
}