using System;
using System.Diagnostics;
using System.Linq;
using Homebase.Business.Infrastructure.Interfaces;
using Windows.ApplicationModel.Background;

namespace Homebase.Business.Infrastructure
{
    internal class BackgroundTaskRegistrar : IBackgroundTaskRegistrar
    {
        public BackgroundTaskRegistration Register(string taskEntryPoint, string name,
            IBackgroundTrigger trigger, IBackgroundCondition condition)
        {
            if (string.IsNullOrWhiteSpace(taskEntryPoint)) throw new ArgumentNullException(nameof(taskEntryPoint));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (trigger == null) throw new ArgumentNullException(nameof(trigger));
            if (condition == null) throw new ArgumentNullException(nameof(condition));

            var existingTask = GetTask(name);
            if (existingTask != null)
                return existingTask;

            var task = CreateTask(taskEntryPoint, name, trigger, condition);

            return task;
        }

        public void Unregister(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            var tasks = BackgroundTaskRegistration.AllTasks
                .Where(t => t.Value.Name == name)
                .Select(t => t.Value as BackgroundTaskRegistration);

            foreach (var task in tasks)
            {
                task.Unregister(true);
            }
        }

        private BackgroundTaskRegistration CreateTask(string taskEntryPoint, string name,
            IBackgroundTrigger trigger, IBackgroundCondition condition = null)
        {
            var builder = new BackgroundTaskBuilder();
            builder.Name = name;
            builder.TaskEntryPoint = taskEntryPoint;
            builder.SetTrigger(trigger);

            if (condition != null)
                builder.AddCondition(condition);

            var task = builder.Register();

            return task;
        }

        private BackgroundTaskRegistration GetTask(string name)
        {
            try
            {
                return BackgroundTaskRegistration.AllTasks
                    .Where(t => t.Value.Name == name)
                    .Select(t => t.Value as BackgroundTaskRegistration)
                    .SingleOrDefault();
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex);

                Unregister(name);

                return null;
            }
        }
    }
}
