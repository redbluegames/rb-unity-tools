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

using UnityEngine;

namespace RedBlueTools
{
	[System.Serializable]
	public class RBAchievement
	{
		public string id { get; private set; }

		public Status status { get; private set; }

		public enum Status
		{
			Unearned = 0,
			EarnedUnsent = 1,
			EarnedSentOnce = 2,
			EarnedSubmitted = 3
		}

		public RBAchievement (string id, int status)
		{
			this.id = id;
			this.status = (Status)status;
		}

		public void MarkAchievementEarned ()
		{
			status = Status.EarnedUnsent;
		}

		public void MarkAchievementSubmitted ()
		{
			status = Status.EarnedSubmitted;
		}

		public void MarkAchievementSent ()
		{
			status = Status.EarnedSentOnce;
		}

		public void SetStatus (Status newStatus)
		{
			status = newStatus;
		}
	}
}