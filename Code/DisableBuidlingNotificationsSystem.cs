
using System.ComponentModel;
using Colossal.Logging;
using Game;
using Game.Prefabs;
using HideBuildingNotification;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace HideBuildingsNotification
{
    internal sealed partial class DisableBuidlingNotificationsSystem : GameSystemBase
    {
        public static ILog log = LogManager.GetLogger($"{nameof(HideBuildingNotification)}").SetShowsErrorsInUI(false);
        private EntityQuery m_Query;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_Query = GetEntityQuery(ComponentType.ReadOnly<NotificationIconDisplayData>());
            RequireForUpdate(m_Query);
        }

        protected override void OnUpdate()
        {
            NativeArray<Entity> m_EntityList = m_Query.ToEntityArray(Allocator.Persistent);
            var m_hideBuildingNotification = Mod.GetHideBuildingNotifications();
            for (int i = 0; i < m_EntityList.Length; i++)
            {
                var entity = m_EntityList[i];
                if (m_hideBuildingNotification)
                {

                    if (
                        EntityManager.HasComponent<NotificationIconDisplayData>(entity) &&
                        !EntityManager.HasComponent<NotificationIconDataDefault>(entity)
                        )
                    {
                        var component = EntityManager.GetComponentData<NotificationIconDisplayData>(entity);
                        var defaultComponent = new NotificationIconDataDefault();
                        defaultComponent.m_MinParams = component.m_MinParams;
                        defaultComponent.m_MaxParams = component.m_MaxParams;
                        component.m_MinParams = 0x0;
                        component.m_MaxParams = 0x0;
                        EntityManager.AddComponentData(entity, defaultComponent);
                        EntityManager.SetComponentData(entity, component);
                    }
                }
                else
                {
                    if (EntityManager.HasComponent<NotificationIconDataDefault>(entity))
                    {
                        var component = EntityManager.GetComponentData<NotificationIconDisplayData>(entity);
                        var defaultComponent = EntityManager.GetComponentData<NotificationIconDataDefault>(entity);
                        component.m_MinParams = defaultComponent.m_MinParams;
                        component.m_MaxParams = defaultComponent.m_MaxParams;
                        EntityManager.SetComponentData(entity, component);
                        EntityManager.RemoveComponent<NotificationIconDataDefault>(entity);
                    }
                }
            }
            m_EntityList.Dispose(); //Release the memory
        }

        protected override void OnDestroy()
        {
            m_Query = GetEntityQuery(ComponentType.ReadOnly<NotificationIconDisplayData>());
            NativeArray<Entity> m_EntityList = m_Query.ToEntityArray(Allocator.Persistent);
            for (int i = 0; i < m_EntityList.Length; i++)
            {
                var entity = m_EntityList[i];
                if (EntityManager.HasComponent<NotificationIconDataDefault>(entity))
                {
                    var component = EntityManager.GetComponentData<NotificationIconDisplayData>(entity);
                    var defaultComponent = EntityManager.GetComponentData<NotificationIconDataDefault>(entity);
                    component.m_MinParams = defaultComponent.m_MinParams;
                    component.m_MaxParams = defaultComponent.m_MaxParams;
                    EntityManager.SetComponentData(entity, component);
                    EntityManager.RemoveComponent<NotificationIconDataDefault>(entity);
                }
            }            
            m_EntityList.Dispose(); //Release the memory
            log.Info("Reset all building notifications to default values before quiting");
            base.OnDestroy();
        }

        private struct NotificationIconDataDefault : IComponentData
        {
            public float2 m_MinParams;
            public float2 m_MaxParams;
        }
    }
}
