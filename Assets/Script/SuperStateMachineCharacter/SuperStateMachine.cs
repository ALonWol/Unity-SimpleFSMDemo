/*
 * original code
 * https://github.com/liangdong-xd/SuperMario64HD/blob/master/Assets/SuperCharacterController/Core/SuperStateMachine.cs
 */
using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class SuperStateMachine : MonoBehaviour
{
    struct State {
        public Action onEnter;
        public Action onUpdate;
        public Action onExit;

        public Enum current;
    }

    State state = new State();

    public Enum currentState {
        get => state.current;
        set {
            ChangingState();
            state.current = value;
            TranslateState();
        }
    }

    public Enum lastState {get; private set;}

    protected float timeEnteredState;

    void Update() {
        OnEarlyUpdate();
        state.onUpdate();
        OnLateUpdate();
    }

    virtual protected void OnEarlyUpdate() {}

    virtual protected void OnLateUpdate() {}

    void ChangingState() {
        lastState = currentState;
        timeEnteredState = Time.time;
    }

    void TranslateState() {
        if (state.onExit != null) {
            state.onExit();
        }

        state.onEnter = GetAction("EnterState");
        state.onUpdate = GetAction("UpdateState");
        state.onExit = GetAction("ExitState");

        if (state.onEnter != null) {
            state.onEnter();
        }
    }

    Dictionary<Enum, Dictionary<string, Action>> cache = new Dictionary<Enum, Dictionary<string, Action>>();

    Action GetAction(string name) {
        Dictionary<string, Action> actions;
        if (!cache.TryGetValue(currentState, out actions)) {
            actions = new Dictionary<string, Action>();
            cache.Add(currentState, actions);
        }

        Action action = null;
        if (!actions.TryGetValue(name, out action)) {
            var methodInfo = GetType().GetMethod(currentState.ToString() + "_" + name, BindingFlags.Instance | BindingFlags.NonPublic);
            if (methodInfo != null) {
                action = (Action)Delegate.CreateDelegate(typeof(Action), this, methodInfo);
            } else {
                action = DoNothing;
            }
            actions.Add(name, action);
        }

        return action;
    }

    static void DoNothing() {}
}
