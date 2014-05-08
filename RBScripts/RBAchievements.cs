/*****************************************************************************
 *  Red Blue Tools are Unity Editor utilities. Some utilities require 3rd party tools.
 *  Copyright (C) 2014 Red Blue Games, LLC
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ****************************************************************************/

namespace RedBlueTools
{
	[System.Serializable]
	public class RBAchievements
	{
		public RBAchievement[] achievements;

		public RBAchievements ()
		{
			achievements = new RBAchievement[5];
			achievements [0] = new RBAchievement (GameIds.ActiveBronzeAchievement, false, false, false);
			achievements [1] = new RBAchievement (GameIds.ActiveSilverAchievement, false, false, false);
			achievements [2] = new RBAchievement (GameIds.ActiveGoldAchievement, false, false, false);
			achievements [3] = new RBAchievement (GameIds.ActivePlaceholder1Achievement, false, false, false);
			achievements [4] = new RBAchievement (GameIds.ActivePlaceholder2Achievement, false, false, false);
		}

		public int Length {
			get {
				return achievements.Length;
			}
			private set {
				Length = value;
			}
		}

		public RBAchievement GetAchievement (int index)
		{
			return achievements [index];
		}

		public RBAchievement GetAchievement (string id)
		{
			for (int i = 0; i < achievements.Length; ++i) {
				if (achievements [i].id.CompareTo (id) == 0) {
					return achievements [i];
				}
			}
			return null;
		}
	}

	[System.Serializable]
	public class RBAchievement
	{
		public string id { get; private set; }

		public bool submitted { get; private set; }

		public bool sentOnce { get; private set; }

		public bool earned { get; private set; }
	
		public RBAchievement (string id, bool submitted, bool sentOnce, bool hasbeenEarned)
		{
			this.id = id;
			this.submitted = submitted;
			this.sentOnce = sentOnce;
			this.earned = hasbeenEarned;
		}

		public void MarkAchievementEarned ()
		{
			earned = true;
		}

		public void MarkAchievementSubmitted ()
		{
			submitted = true;
		}

		public void SetAchievementSent (bool sentStatus)
		{
			sentOnce = sentStatus;
		}
	}
}