using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour, ISaveable
{
    #region Editor Fields
    [SerializeField] private GameObject _equipmentPrefab = null;
    #endregion

    #region Fields
    private List<EquipmentField> _equipments = new List<EquipmentField>();
    #endregion

    #region Functions
    public void AddField(Equipment equipment, bool asFirst = false)
    {
        if (Instantiate(_equipmentPrefab, this.transform).TryGetComponent(out EquipmentField equipField))
        {
            _equipments.Add(equipField);
            equipField.Initialize(equipment, asFirst);
        }
    }
    public void RemoveField(EquipmentField field)
    {
        _equipments.Remove(field);

        Destroy(field.gameObject);
    }

    public void Load(Character sheet)
    {
        EquipmentField.Manager = this;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        _equipments.Clear();

        //Spawn first field as empty
        AddField(new Equipment(), true);

        for (int i = 0; i < sheet.Equipments.Count; i++)
        {
            AddField(sheet.Equipments[i]);
        }
    }

    public void Save(Character sheet)
    {
        sheet.Equipments.Clear();

        for (int i = 1; i < _equipments.Count; i++)
        {
            EquipmentField field = _equipments[i];

            if (!string.IsNullOrWhiteSpace(field.name))
            {
                sheet.Equipments.Add(field.GetEquipment());
            }
        }
    }
    #endregion
}
