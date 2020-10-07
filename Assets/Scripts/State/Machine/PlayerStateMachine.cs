using System;
using System.Collections.Generic;
using State.Attack;

namespace State.Machine {
	public class PlayerStateMachine : StateMachine {		

		private Type currentStateType;
		private readonly Dictionary<Type, State> states;
		private readonly Player player;

		public PlayerStateMachine(Player player) {
			this.player = player;
			states = new Dictionary<Type, State>();
		}

		public void Add(State state) {
			var type = state is SpecialAttackState ? state.GetType().BaseType : state.GetType();
			if (!states.ContainsKey(type))
				states.Add(type, state);
		}

		public void Toggle<TS>() where TS : State {
			var type = typeof(TS);
			if (!states.ContainsKey(type)) return;
			
			if (currentStateType != null)
				states[currentStateType].Exit(player);
			currentStateType = type;
			states[currentStateType].Enter(player);
		}

		public void Resume() {
			states[currentStateType].Resume(player);
		}
	}
}