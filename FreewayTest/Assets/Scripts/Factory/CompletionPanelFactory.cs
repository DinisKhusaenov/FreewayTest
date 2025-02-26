using UI;
using UnityEngine;

namespace Factory
{
    public class CompletionPanelFactory
    {
        private CompletionPanel _completionPanel;
        private Transform _parent;

        public CompletionPanelFactory(CompletionPanel completionPanel, Transform parent)
        {
            _completionPanel = completionPanel;
            _parent = parent;
        }

        public CompletionPanel Get()
        {
            var panel = Object.Instantiate(_completionPanel, _parent);
            panel.Hide();

            return panel;
        }
    }
}