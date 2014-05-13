﻿/*****************************************************************************
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

		public RBAchievements (string[] achievementIDs)
		{
			achievements = new RBAchievement[achievementIDs.Length];
			for(int i = 0; i < achievementIDs.Length; i++)
			{
				achievements[i] = new RBAchievement( achievementIDs[i], false, false, false);
			}
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
}