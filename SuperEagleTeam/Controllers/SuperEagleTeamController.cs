
using System;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SuperEagleTeam.Data;
using SuperEagleTeam.Models;

namespace SuperEagleTeam.Controllers
{
	[Route("api/SuperEagleTeam")]
	[ApiController]
	public class SuperEagleTeamController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<IEnumerable<SuperEagleTeamModel>> GetPlayer()
		{
            return Ok(TeamData.player);
		}


		[HttpGet("{id:int}", Name = "GetSuperEagleTeam")]
		[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<SuperEagleTeamModel>> GetPlayer(int id)
		{
			if(id == null || id == 0)
			{
				return BadRequest();
			}

			var getPlayer = TeamData.player.FirstOrDefault(c => c.id == id);

			if(getPlayer == null)
			{
				return NotFound();
			}

			return Ok(getPlayer);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<SuperEagleTeamModel> CreateSuperEagleTeam([FromBody] SuperEagleTeamModel team)
		{
			if(team == null)
			{
				return BadRequest();
			}

			if(TeamData.player.FirstOrDefault(c => c.PlayerName == team.PlayerName) != null)
			{
				ModelState.AddModelError("Custom error", "Play name already exist!");
				return BadRequest(ModelState);
			}

			if(team.id > 0)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			team.id = TeamData.player.OrderByDescending(c => c.id).FirstOrDefault().id + 1;
			TeamData.player.Add(team);

			return CreatedAtRoute("GetSuperEagleTeam", new { id = team.id }, team);
		}

		[HttpDelete("{id:int}", Name = "DeleteSuperEagleTeam")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteSuperEagleTeam(int id)
		{
			if(id == 0)
			{
				return BadRequest();
			}

			var team = TeamData.player.FirstOrDefault(c => c.id == id);

			if(team == null)
			{
				return NotFound();
			}

			TeamData.player.Remove(team);
			return NoContent();
		}

        [HttpPut("{id:int}", Name = "UpdateSuperEagleTeam")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateSuperEagleTeam(int id, [FromBody] SuperEagleTeamModel team)
		{
			if(id != team.id || id == 0 || id == null)
			{
				return BadRequest();
			}

			var superEagleTeam = TeamData.player.FirstOrDefault(c => c.id == id);

			if(superEagleTeam == null)
			{
				return NotFound();
			}

			superEagleTeam.ShiritNumber = team.ShiritNumber;
			superEagleTeam.PlayerName = team.PlayerName;
			superEagleTeam.PlayerPosition = team.PlayerPosition;
			superEagleTeam.CurrentClub = team.CurrentClub;
			superEagleTeam.ClubCountry = team.ClubCountry;
			superEagleTeam.IsStartingEleven = team.IsStartingEleven;

			return NoContent();

        }

		[HttpPatch("{id:int}", Name = "PartialUpdateSuperEagleTeam")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PartialUpdate(int id, JsonPatchDocument<SuperEagleTeamModel> jsonPatch)
		{
			if(id == null || jsonPatch == null)
			{
				return BadRequest();
			}

			var team = TeamData.player.FirstOrDefault(c => c.id == id);

			if(team == null)
			{
				return NotFound();
			}

			jsonPatch.ApplyTo(team, ModelState);

			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			return NoContent();
		}

    }
}

