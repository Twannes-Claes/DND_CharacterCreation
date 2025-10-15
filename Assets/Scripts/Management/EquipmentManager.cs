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

    #region LifeCycle
    #endregion

    #region Functions

    public void AddField(Equipment equipment)
    {
        if (Instantiate(_equipmentPrefab, this.transform).TryGetComponent(out EquipmentField equipField))
        {
            _equipments.Add(equipField);
            equipField.Initialize(equipment);
        }
    }
    public void RemoveField(EquipmentField field)
    {
        _equipments.Remove(field);

        Destroy(field.gameObject);
    }

    public void Load(Character sheet)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        _equipments.Clear();
        EquipmentField.Manager = this;

        for (int i = 0; i < sheet.Equipments.Count + 1; i++)
        {
            if (Instantiate(_equipmentPrefab, this.transform).TryGetComponent(out EquipmentField equipField))
            {
                _equipments.Add(equipField);

                if (i == 0)
                {
                    equipField.SetFirst();
                    continue;
                }

                equipField.Initialize(sheet.Equipments[i-1]);
            }
        }
    }

    public void Save(Character sheet)
    {
        sheet.Equipments.Clear();

        for (int i = 1; i < _equipments.Count; i++)
        {
            EquipmentField field = _equipments[i];
            sheet.Equipments.Add(field.GetEquipment());
        }
    }
    #endregion
}
