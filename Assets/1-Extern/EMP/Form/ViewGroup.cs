//
// MIT License
// Copyright (c) EMP - https://github.com/SchmobertRitz/EMP-Unity
//
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EMP.Forms
{
    public abstract class ViewGroup<T> : View
            where T : ViewGroup<T>
    {
        protected List<View> views = new List<View>();

        public virtual T Add(View view)
        {
            views.Add(view);
            Dirty = true;
            view.Parent = this;
            return (T)this;
        }

        public virtual T Remove(View view)
        {
            if (views.Remove(view))
            {
                view.Parent = null;
                Dirty = true;
            }
            return (T)this;
        }

        public override void Draw()
        {
            views.ForEach(view => {
                view.Draw();
                //EditorGUI.DrawRect(view.Rect, new Color(0, 0, 0, 0.1f));
            });
        }

        public override Validator.Issue[] Validate()
        {
            List<Validator.Issue> issues = new List<Validator.Issue>();
            views.ForEach(v => issues.AddRange(v.Validate()));
            return issues.ToArray();
        }
    }
}
