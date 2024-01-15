using System;
using System.ComponentModel.DataAnnotations;

namespace SuperEagleTeam.Models
{
	public class SuperEagleTeamModel
	{
		public int id { get; set; }

		public int ShiritNumber { get; set; }

		public required string PlayerName { get; set; }
		public required string PlayerPosition { get; set; }
		public required string CurrentClub { get; set; }
		public required string ClubCountry { get; set; }	
		public bool IsStartingEleven { get; set; }
	}
}

