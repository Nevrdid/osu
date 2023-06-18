// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.UI;

namespace osu.Game.Screens.Play.HUD
{
    public partial class KeyCounterController : CompositeComponent, IKeybindingListener
    {
        public readonly Bindable<bool> IsCounting = new BindableBool(true);

        public event Action<InputTrigger>? OnNewTrigger;

        private readonly Container<InputTrigger> triggers;

        public IReadOnlyList<InputTrigger> Triggers => triggers;

        public KeyCounterController()
        {
            InternalChild = triggers = new Container<InputTrigger>();
        }

        public void Add(InputTrigger trigger)
        {
            triggers.Add(trigger);
            trigger.IsCounting.BindTo(IsCounting);
            OnNewTrigger?.Invoke(trigger);
        }

        public void AddRange(IEnumerable<InputTrigger> inputTriggers) => inputTriggers.ForEach(Add);
        public override bool HandleNonPositionalInput => true;

        public override bool HandlePositionalInput => true;

        #region IKeybindingListener

        bool IKeybindingListener.CanHandleKeybindings => true;

        void IKeybindingListener.Setup<T>(IEnumerable<T> actions)
            => AddRange(actions.Select(a => new KeyCounterActionTrigger<T>(a)));

        void IKeybindingListener.OnPressed<T>(KeyBindingPressEvent<T> action)
        {
        }

        void IKeybindingListener.OnReleased<T>(KeyBindingReleaseEvent<T> action)
        {
        }

        #endregion
    }
}
