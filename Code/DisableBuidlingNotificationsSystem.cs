
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Colossal.Entities;
using Colossal.Json;
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
            m_Query = GetEntityQuery(
                ComponentType.ReadOnly<NotificationIconDisplayData>(),
                ComponentType.ReadOnly<NotificationIconData>()
                );
            RequireForUpdate(m_Query);
        }

        protected override void OnUpdate()
        {
            NativeArray<Entity> m_EntityList = m_Query.ToEntityArray(Allocator.Persistent);
            List<String> iconToKeep = new List<String>
            {
                "Battery Empty",
                "No Hospital Supplies",
                "No Emergency Shelter Supplies",
                "Condemned",
                "Rent Too High",
                "Pipeline Not Connected - Sewage",
                "Water Not Enough Production Notification",
                "Sewage Not Enough Production Notification",
                "Not Enough Groundwater Notification",
                "Not Enough Surface Water Notification",
                "Dirty Water Pump Notification",
                "Powerline Not Connected - Low",
                "Electricity Building Bottleneck Notification",
                "Electricity Not Enough Production Notification",
                "Electricity Transformer Out of Capacity",
                "Electricity Not Enough Connected Notification",
                "No Watercraft Access",
                "MissingUneducatedWorkers",
                "MissingEducatedWorkers",
                "No Inputs",
                "No Customers",
                "Air Pollution",
                "Noise Pollution",
                "Ground Pollution",
                "No Car Access",
                "No Cargo Access",
                "No Pedestrian Access",
                "No Road Access",
                "No Track Access",
                "No Water",
                "Not Enough Money",
                "Not On Shoreline",
                "Overlap Existing",
                "Pathfind Failed",
                "Short Distance",
                "Small Area",
                "Tight Curve",
                "Steep Slope",
                "Exceeds Lot Limits",
                "Not On Border",
                "No Train Access",
                "No Groundwater",
                "Abandoned Collapsed",
                "Abandoned",
                "Ambulance Notification",
                "Burned Down",
                "Crime Scene",
                "Destroyed",
                "Dirty Water",
                "Electricity Notification",
                "Facility Full",
                "Garbage Notification",
                "Hearse Notification",
                "No Fuel Notification",
                "Pipeline Not Connected",
                "Powerline Not Connected",
                "Sewage Notification",
                "Water Damage",
                "Water Destroyed",
                "Water Notification",
                "Weather Damage",
                "Weather Destroyed",
                "Electricity Bottleneck Notification",
                "Traffic Bottleneck Notification",
                "Turned Off",
                "Dead End",
                "No Vehicles",

            };

            var m_hideBuildingNotification = Mod.GetHideBuildingNotifications();
            for (int i = 0; i < m_EntityList.Length; i++)
            {
                var entity = m_EntityList[i];
                var prefabName = base.World.GetOrCreateSystemManaged<PrefabSystem>().GetPrefabName(entity);
                if (m_hideBuildingNotification && iconToKeep.Contains(prefabName))
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

/*
 Not hidded :                 
 "No Outside Connection",
    "Building Level Up",
    "Selected",
    "Followed",
    "Happy Face",
    "Sad Face",
    "ValentineHeart",
    "Road Outside Connection",
    "Train Outside Connection",
    "Ship Outside Connection",
    "Airplane Outside Connection",
    "Electricity Outside Connection",
    "Water Pipe Outside Connection",
    "Creature Spawner",
    "Passenger Transport",
    "Cargo Transport",
    "Taxi",
    "Ambulance",
    "Fire Engine",
    "Garbage Truck",
    "Hearse",
    "Park Maintenance Vehicle",
    "Police Car",
    "Post Van",
    "Prisoner Transport",
    "Road Maintenance Vehicle",
    "Evacuating Transport",
    "Admin Building",
    "Deathcare Facility",
    "Disaster Facility",
    "Emergency Shelter",
    "Fire Station",
    "Firewatch Tower",
    "Fresh Water Building",
    "Garbage Facility",
    "Hospital",
    "Park Maintenance Depot",
    "Park",
    "Parking Facility",
    "Police Station",
    "Post Facility",
    "Power Plant",
    "Prison",
    "Research Facility",
    "Road Maintenance Depot",
    "School",
    "Sewage Building",
    "Telecom Facility",
    "Transformer",
    "Transport Depot",
    "Transport Station",
    "Welfare Office",
    "Signature Residential",
    "Signature Commercial",
    "Signature Industrial",
    "Signature Office",
    "Extractor Building",
    "Bus Stop",
    "Taxi Stand",
    "Train Stop",
    "Tram Stop",
    "Subway Stop",
    "Ship Stop",
    "Airplane Stop",
    "Mail Box",
    "Cargo Train Stop",
    "Cargo Ship Stop",
    "Cargo Airplane Stop",
    "Already Exists",
    "Already Upgraded",
    "Exceeds City Limits",
    "Fire Notification",
    "In Water",
    "Invalid Shape",
    "Long Distance",
    "Low Elevation",
    "Track Not Connected",
    "Traffic Accident",

List prefab names:
    "Battery Empty",
    "No Hospital Supplies",
    "No Emergency Shelter Supplies",
    "Condemned",
    "Rent Too High",
    "Pipeline Not Connected - Sewage",
    "Water Not Enough Production Notification",
    "Sewage Not Enough Production Notification",
    "Not Enough Groundwater Notification",
    "Not Enough Surface Water Notification",
    "Dirty Water Pump Notification",
    "Powerline Not Connected - Low",
    "Electricity Building Bottleneck Notification",
    "Electricity Not Enough Production Notification",
    "Electricity Transformer Out of Capacity",
    "Electricity Not Enough Connected Notification",
    "No Watercraft Access",
    "MissingUneducatedWorkers",
    "MissingEducatedWorkers",
    "No Inputs",
    "No Customers",
    "No Outside Connection",
    "Building Level Up",
    "Selected",
    "Followed",
    "Happy Face",
    "Sad Face",
    "ValentineHeart",
    "Road Outside Connection",
    "Train Outside Connection",
    "Ship Outside Connection",
    "Airplane Outside Connection",
    "Electricity Outside Connection",
    "Water Pipe Outside Connection",
    "Creature Spawner",
    "Passenger Transport",
    "Cargo Transport",
    "Taxi",
    "Ambulance",
    "Fire Engine",
    "Garbage Truck",
    "Hearse",
    "Park Maintenance Vehicle",
    "Police Car",
    "Post Van",
    "Prisoner Transport",
    "Road Maintenance Vehicle",
    "Evacuating Transport",
    "Admin Building",
    "Deathcare Facility",
    "Disaster Facility",
    "Emergency Shelter",
    "Fire Station",
    "Firewatch Tower",
    "Fresh Water Building",
    "Garbage Facility",
    "Hospital",
    "Park Maintenance Depot",
    "Park",
    "Parking Facility",
    "Police Station",
    "Post Facility",
    "Power Plant",
    "Prison",
    "Research Facility",
    "Road Maintenance Depot",
    "School",
    "Sewage Building",
    "Telecom Facility",
    "Transformer",
    "Transport Depot",
    "Transport Station",
    "Welfare Office",
    "Signature Residential",
    "Signature Commercial",
    "Signature Industrial",
    "Signature Office",
    "Extractor Building",
    "Bus Stop",
    "Taxi Stand",
    "Train Stop",
    "Tram Stop",
    "Subway Stop",
    "Ship Stop",
    "Airplane Stop",
    "Mail Box",
    "Cargo Train Stop",
    "Cargo Ship Stop",
    "Cargo Airplane Stop",
    "Already Exists",
    "Already Upgraded",
    "Exceeds City Limits",
    "Fire Notification",
    "In Water",
    "Invalid Shape",
    "Long Distance",
    "Low Elevation",
    "No Car Access",
    "No Cargo Access",
    "No Pedestrian Access",
    "No Road Access",
    "No Track Access",
    "No Water",
    "Not Enough Money",
    "Not On Shoreline",
    "Overlap Existing",
    "Pathfind Failed",
    "Short Distance",
    "Small Area",
    "Tight Curve",
    "Steep Slope",
    "Exceeds Lot Limits",
    "Not On Border",
    "No Train Access",
    "No Groundwater",
    "Abandoned Collapsed",
    "Abandoned",
    "Ambulance Notification",
    "Burned Down",
    "Crime Scene",
    "Destroyed",
    "Dirty Water",
    "Electricity Notification",
    "Facility Full",
    "Garbage Notification",
    "Hearse Notification",
    "No Fuel Notification",
    "Pipeline Not Connected",
    "Powerline Not Connected",
    "Sewage Notification",
    "Track Not Connected",
    "Traffic Accident",
    "Water Damage",
    "Water Destroyed",
    "Water Notification",
    "Weather Damage",
    "Weather Destroyed",
    "Electricity Bottleneck Notification",
    "Traffic Bottleneck Notification",
    "Turned Off",
    "Dead End",
    "No Vehicles",
    "Air Pollution",
    "Noise Pollution",
    "Ground Pollution"
];
 */