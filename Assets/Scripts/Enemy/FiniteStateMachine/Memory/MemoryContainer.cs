using System;
using System.Collections.Generic;
using System.Linq;

namespace CP.AILibrary.Storage
{
    public class MemoryContainer
    {
        private SerializableDictionary<string, Data> _memory = new SerializableDictionary<string, Data>();
        protected SerializableDictionary<string, Data> Memory
        {
            get
            {
                if (_memory == null)
                    _memory = new SerializableDictionary<string, Data>();
                return _memory;
            }
            set => _memory = value;
        }

        public event Action<Data> onDataAdded;
        public event Action<Data> onDataChanged;
        public event Action<Data> onDataRemoved;

        public object this[string dataName]
        {
            get => Memory[dataName] != null ? Memory[dataName].value : null;
            set => SetValue(dataName, value);
        }

        public XmlData WriteXml()
        {
            return Memory.WriteXml(Memory);
        }

        public void ReadXml(MemoryContainer mem, XmlData xmlData)
        {
            Memory.ReadXml(mem, xmlData);
        }

        public MemoryContainer() { }

        public MemoryContainer(string Name)
        {
            
        }

        //Setups do not need bool returns however they are in place for future upgrades?? 
        public bool Setup()
        {
            return true;
        }

        public bool Setup(string Name)
        {
            return true;
        }
        
        public bool AddData(string dataName, object value)
        {
            if (value == null)
            {
                return false;
            }

            if(AddData(dataName, value.GetType(), false))
            {
                var newData = GetData(dataName);

                if (newData != null)
                {
                    newData.value = value;
                    onDataAdded?.Invoke(newData);
                    return true;
                } else
                {
                    return false;
                }
            } else
            {
                return false;
            }
        }
        
        public bool AddData(string dataName, Type value, bool triggerInvoke = true)
        {
            Type type = value;

            if (Memory.ContainsKey(dataName))
            {
                Data existing = GetData(dataName);
                
                return false;
            }

            var dataType = typeof(Data<>).MakeGenericType(type);
            var newData = (Data)Activator.CreateInstance(dataType);

            newData.Name = dataName;

            if(type == typeof(Type))
            {
                newData.value = typeof(System.Boolean);
            } else if(type == typeof(string))
            {
                newData.value = "";
            } else
            {
                newData.value = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
            }
            
            Memory.Add(dataName, newData);
            if(triggerInvoke)
                onDataAdded?.Invoke(newData);
            return true;
        }
        
        public bool RemoveData(string dataName)
        {
            if (Memory.TryGetValue(dataName, out Data data) && Memory.Remove(dataName))
            {
                onDataRemoved?.Invoke(data);
                return true;
            }
            return false;
        }

        public bool SetValue(string dataName, object value)
        {
            if (!Memory.ContainsKey(dataName))
            {
                return AddData(dataName, value);
            }

            Memory[dataName].value = value;
            onDataChanged?.Invoke(Memory[dataName]);
            return true;
        }

        public bool SetDataName(string dataName, string newDataName)
        {
            Memory.ReplaceKey(dataName, newDataName);
            Memory[newDataName].Name = newDataName;

            onDataChanged?.Invoke(Memory[newDataName]);

            return true;
        }

        public T GetValue<T>(string dataName)
        {
            if (!Memory.ContainsKey(dataName))
            {
                return default;
            }

            T val = (Memory[dataName] as Data<T>).value;

            if (val == null)
                val = (T)Memory[dataName].value;

            if (val == null)
            {
                return default;
            }

            return val;
        }

        public Data GetData(string dataName)
        {
            if (Memory.TryGetValue(dataName, out Data var))
                return var;
            return null;
        }

        public Data GetData(Guid ID)
        {
            return Memory.Where(w => w.Value.ID == ID).First().Value;
        }

        public bool CheckDataExist(params string[] dataNames)
        {
            if (dataNames.Length == 0)
            {
                return false;
            }
            else if (dataNames.Length == 1)
            {
                bool val = Memory.ContainsKey(dataNames[0]);

                return val;
            }
            else
            {
                List<string> missingVars = new List<string>();

                foreach (string dataName in dataNames)
                    if (!Memory.ContainsKey(dataName))
                        missingVars.Add(dataName);

                if (missingVars.Count != 0)
                {
                    return false;
                }
                else
                    return true;
            }
        }

        public Data<T> GetData<T>(string dataName)
        {
            return (Data<T>)GetData(dataName);
        }

        public string[] GetDataNames()
        {
            return Memory.Keys.ToArray();
        }

        public string[] GetDataNames(Type ofType)
        {
            return Memory.Values.Where(v => v.dataType == ofType).Select(v => v.Name).ToArray();
        }

        public Data[] Values()
        {
            return Memory.Values.ToArray();
        }
    }
}