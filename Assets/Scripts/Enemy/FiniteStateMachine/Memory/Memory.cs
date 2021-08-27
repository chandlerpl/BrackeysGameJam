using System;
using UnityEngine;

namespace CP.AILibrary.Storage
{
    [Serializable]
    public class Memory : IMemory, ISerializationCallbackReceiver
    {
        public XmlData serialisationData = new XmlData();

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (serialisationData.reserialise)
            {
                serialisationData = MemoryContainer.WriteXml();
                serialisationData.reserialise = false;
            }
        }

        // load dictionary from lists
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _memoryContainer.Setup("Serialisation Memory");
            MemoryContainer.ReadXml(MemoryContainer, serialisationData);
        }

        private MemoryContainer _memoryContainer = new MemoryContainer();
        public MemoryContainer MemoryContainer
        {
            get
            {
                if (_memoryContainer == null)
                {
                    _memoryContainer = new MemoryContainer();
                    _memoryContainer.Setup("Memory");

                }
                return _memoryContainer;
            }
            protected set => _memoryContainer = value;
        }
        
        public event Action<Data> OnDataAdded { add { MemoryContainer.onDataAdded += value; } remove { MemoryContainer.onDataAdded -= value; } }
        public event Action<Data> OnDataChanged { add { MemoryContainer.onDataChanged += value; } remove { MemoryContainer.onDataChanged -= value; } }
        public event Action<Data> OnDataRemoved { add { MemoryContainer.onDataRemoved += value; } remove { MemoryContainer.onDataRemoved -= value; } }

        public object this[string dataName]
        {
            get => MemoryContainer[dataName] != null ? MemoryContainer[dataName] : null;
            set => SetValue(dataName, value);
        }

        //Setups do not need bool returns however they are in place for future upgrades?? 
        public bool Setup()
        {
            MemoryContainer.Setup();

            return true;
        }

/*        public XmlData GetSerialisationData()
        {
            return null;
        }*/

        public MemoryContainer GetMemoryContainer()
        {
            return MemoryContainer;
        }

        public bool Setup(string Name)
        {
            MemoryContainer.Setup(Name);

            return true;
        }

        public bool AddData(string dataName, object value)
        {
            return MemoryContainer.AddData(dataName, value);
        }
        
        public bool AddData(string dataName, Type value)
        {
            return MemoryContainer.AddData(dataName, value);
        }

        public bool RemoveData(string dataName)
        {
            return MemoryContainer.RemoveData(dataName);
        }

        public bool SetValue(string dataName, object value)
        {
            return MemoryContainer.SetValue(dataName, value);
        }

        public bool SetDataName(string dataName, string newDataName)
        {
            return MemoryContainer.SetDataName(dataName, newDataName);
        }

        public T GetValue<T>(string dataName)
        {
            return MemoryContainer.GetValue<T>(dataName);
        }

        public Data GetData(string dataName)
        {
            return MemoryContainer.GetData(dataName);
        }

        public Data GetData(Guid ID)
        {
            return MemoryContainer.GetData(ID);
        }

        public bool CheckDataExist(params string[] dataNames)
        {
            return MemoryContainer.CheckDataExist(dataNames);
        }

        public Data<T> GetData<T>(string dataName)
        {
            return MemoryContainer.GetData<T>(dataName);
        }

        public string[] GetDataNames()
        {
            return MemoryContainer.GetDataNames();
        }

        public string[] GetDataNames(Type ofType)
        {
            return MemoryContainer.GetDataNames(ofType);
        }

        public Data[] Values()
        {
            return MemoryContainer.Values();
        }
    }
}