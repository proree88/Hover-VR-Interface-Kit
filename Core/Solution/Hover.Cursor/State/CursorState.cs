﻿using System;
using System.Collections.Generic;
using Hover.Common.Input;
using Hover.Common.State;
using Hover.Cursor.Custom;
using Hover.Cursor.Input;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public class CursorState : ICursorState {

		public CursorType Type { get; private set; }
		public bool IsInputAvailable { get; private set; }
		public Vector3 Position { get; private set; }
		public float Size { get; private set; }
		public float DisplayStrength { get; private set; }

		private readonly IInputCursor vInputCursor;
		private readonly ICursorSettings vSettings;
		private readonly Transform vBaseTx;

		private readonly List<IBaseItemInteractionState> vInteractItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorState(IInputCursor pInputCursor, ICursorSettings pSettings, Transform pBaseTx) {
			vInputCursor = pInputCursor;
			vSettings = pSettings;
			vBaseTx = pBaseTx;
			vInteractItems = new List<IBaseItemInteractionState>();

			Type = vInputCursor.Type;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float GetMaxHighlightProgress() {
			float maxProg = 0;

			foreach ( IBaseItemInteractionState interact in vInteractItems ) {
				maxProg = Math.Max(maxProg, interact.MaxHighlightProgress);
			}

			return maxProg;
		}

		/*--------------------------------------------------------------------------------------------*/
		public float GetMaxSelectionProgress() {
			float maxProg = 0;

			foreach ( IBaseItemInteractionState interact in vInteractItems ) {
				maxProg = Math.Max(maxProg, interact.SelectionProgress);
			}

			return maxProg;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetWorldPosition() {
			return vBaseTx.TransformPoint(Position);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void ClearInteractions() {
			vInteractItems.Clear();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddInteractions(IEnumerable<IBaseItemInteractionState> pInteractions) {
			vInteractItems.AddRange(pInteractions);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput(float pDisplayStren) {
			DisplayStrength = pDisplayStren;

			IsInputAvailable = vInputCursor.IsAvailable;
			Size = vInputCursor.Size;

			Position = vInputCursor.Position+
				vInputCursor.Rotation*Vector3.back*vSettings.CursorForwardDistance;
		}

	}

}
