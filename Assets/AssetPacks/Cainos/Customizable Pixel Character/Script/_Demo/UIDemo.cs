using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Cainos.CustomizablePixelCharacter
{
    public class UIDemo : MonoBehaviour
    {
        public GameObject areaControl;
        public GameObject areaCustomization;
        public GameObject areaPreset;
        [Space]
        public UISelector selectorGender;
        public UISelector selectorHairstyle;
        public UISelector selectorHairColor;
        public UISelector selectorEyeColor;
        public UISelector selectorHat;
        public UISelector selectorFacewear;
        public UISelector selectorCloth;
        public UISelector selectorPants;
        public UISelector selectorSocks;
        public UISelector selectorShoes;
        public UISelector selectorBack;
        public UISelector selectorWeapon;
        public TMP_Dropdown dropdownPreset;
        [Space]
        public PixelCharacter character;
        [Space]
        public List<Material> hairMaleMats;
        public List<Material> hairFemaleMats;
        public List<Texture> hairColorTexs;
        public List<Material> eyeMats;
        public List<Material> eyeBaseMats;
        public List<Material> hatMats;
        public List<Material> facewearMats;
        public List<Material> clothMats;
        public List<Material> pantsMats;
        public List<Material> socksMats;
        public List<Material> shoesMats;
        public List<Material> backMats;
        public List<Material> bodyMats;
        public List<GameObject> weapons;

        public List<PixelCharacter> presets;
        [Space]
        public PixelCharacter startPreset;

        private bool isUIShown = true;
        private int hairstyleIndex = 0;

        public void OnGenderChanged( int index)
        {
            character.BodyMaterial = bodyMats[index];
            character.EyeBaseMaterial = eyeBaseMats[index];

            selectorHairstyle.Index = hairstyleIndex;
        }

        public void OnHairstyleChanged ( int index )
        {
            hairstyleIndex = index;

            if (selectorGender.Index == 0) character.HairMaterial = hairMaleMats[index];
            else
            if (selectorGender.Index == 1) character.HairMaterial = hairFemaleMats[index];
        }

        public void OnHairColorChanged ( int index)
        {
            character.HairRampTexture = hairColorTexs[index];
        }

        public void OnEyeColorChanged ( int index)
        {
            character.EyeMaterial = eyeMats[index];
        }

        public void OnHatChanged(int index)
        {
            character.HatMaterial = hatMats[index];

            int l = selectorHat.items[index].Length - 1;
            if (selectorHat.items[index][l] == 'C') character.ClipHair = true;
            else character.ClipHair = false;
        }

        public void OnFacewearChanged(int index)
        {
            character.FacewearMaterial = facewearMats[index];
        }

        public void OnClothChanged ( int index)
        {
            character.ClothMaterial = clothMats[index];
        }

        public void OnPantsChanged ( int index )
        {
            character.PantsMaterial = pantsMats[index];
        }

        public void OnSocksChanged(int index)
        {
            character.SocksMaterial = socksMats[index];
        }

        public void OnShoesChanged(int index)
        {
            character.ShoesMaterial = shoesMats[index];
        }

        public void OnBackChanged(int index)
        {
            character.BackMaterial = backMats[index];
        }

        public void OnWeaponChanged ( int index)
        {
            character.AddWeapon(weapons[index] , true);
        }

        public void OnExpressionChanged ( int index)
        {
            character.Expression = (PixelCharacter.ExpressionType) index;
        }

        public void OnAttackActionChanged ( int index)
        {
            character.AttackAction = (PixelCharacter.AttackActionType)index;
        }

        public void OnDropWeapon()
        {
            character.DropWeapon();
            selectorWeapon.Index = 0;
        }

        public void OnKillRevive()
        {
            var cc = character.GetComponent<PixelCharacterController>();
            cc.IsDead = !cc.IsDead;

            if (cc.IsDead == true) selectorWeapon.Index = 0;
        }

        public void OnInjureFront()
        {
            character.InjuredFront();
        }

        public void OnInjureBack()
        {
            character.InjuredBack();
        }

        public void OnPresetChanged ( int index)
        {
            selectorGender.Index = GetGenderIndex ( presets[index].BodyMaterial );

            selectorHairstyle.Index = GetHairstyleIndex( presets[index].HairMaterial );
            selectorHairColor.Index = GetHairColorIndex( presets[index].HairMaterial );

            selectorEyeColor.Index = eyeMats.IndexOf(presets[index].EyeMaterial);
            selectorHat.Index = hatMats.IndexOf( presets[index].HatMaterial );
            selectorFacewear.Index = facewearMats.IndexOf(presets[index].FacewearMaterial);
            selectorCloth.Index = clothMats.IndexOf ( presets[index].ClothMaterial );
            selectorPants.Index = pantsMats.IndexOf( presets[index].PantsMaterial );
            selectorSocks.Index = socksMats.IndexOf( presets[index].SocksMaterial );
            selectorShoes.Index = shoesMats.IndexOf( presets[index].ShoesMaterial );
            selectorBack.Index = backMats.IndexOf( presets[index].BackMaterial );

            selectorWeapon.Index = GetWeaponIndex( presets[index].Weapon );
        }

        public void ToggleUI()
        {
            isUIShown = !isUIShown;

            areaControl.SetActive(isUIShown);
            areaCustomization.SetActive(isUIShown);
            areaPreset.SetActive(isUIShown);
        }

        public void Reset()
        {
            SceneManager.LoadScene(0);
        }

        private void Start()
        {
            //setup selectors
            for (int i = 0; i < hairColorTexs.Count; i++) selectorHairColor.items.Add(GetName(hairColorTexs[i].name));

            for ( int i = 0; i < eyeMats.Count; i++) selectorEyeColor.items.Add( GetName( eyeMats[i].name ));
            for ( int i = 0; i < hatMats.Count; i++) selectorHat.items.Add( GetName( hatMats[i].name ));
            for (int i = 0; i < facewearMats.Count; i++) selectorFacewear.items.Add(GetName(facewearMats[i].name));
            for ( int i = 0; i < clothMats.Count; i++) selectorCloth.items.Add( GetName( clothMats[i].name ));
            for (int i = 0; i < pantsMats.Count; i++) selectorPants.items.Add(GetName(pantsMats[i].name));
            for (int i = 0; i < socksMats.Count; i++) selectorSocks.items.Add(GetName(socksMats[i].name));
            for (int i = 0; i < shoesMats.Count; i++) selectorShoes.items.Add(GetName(shoesMats[i].name));
            for (int i = 0; i < backMats.Count; i++) selectorBack.items.Add(GetName(backMats[i].name));

            for (int i = 0; i < weapons.Count; i++) selectorWeapon.items.Add(GetName(weapons[i].name));

            //setup preset dropdown
            List<string> presetOptions = new List<string>();
            for (int i = 0; i < presets.Count; i++) presetOptions.Add(GetName(presets[i].gameObject.name));
            dropdownPreset.AddOptions(presetOptions);

            //set start preset
            dropdownPreset.value = presets.IndexOf(startPreset);
        }

        //Get the last section from material or gameobject name
        private string GetName( string rawName )
        {
            string[] s = rawName.Split('-');
            return s[s.Length - 1].TrimStart (' ');
        }

        //get corresponding gender selector index from body material
        private int GetGenderIndex ( Material mat)
        {
            string[] s = mat.name.Split('-');
            string n = s[s.Length - 1].Trim(' ');

            if (n == "Male") return 0;
            else return 1;
        }

        //get corresponding hairstyle selector index from hair material
        private int GetHairstyleIndex ( Material mat)
        {
            string[] s = mat.name.Split('-');
            string n = s[s.Length - 2].Trim(' ');
            s = n.Split(' ');
            n = s[s.Length - 1];

            int l = selectorHairstyle.items.Count;
            for ( int i = 0; i < l; i++)
            {
                int sl = selectorHairstyle.items[i].Length;
                if ( n[0] == selectorHairstyle.items[i][sl-1] )
                {
                    return i;
                }
            }

            return 0;
        }

        //get corresponding hair color selector index from hair material
        private int GetHairColorIndex(Material mat)
        {
            string[] s = mat.name.Split('-');
            string n = s[s.Length - 1].Trim(' ');

            int l = selectorHairColor.items.Count;
            for (int i = 0; i < l; i++)
            {
                if (n == selectorHairColor.items[i])
                {
                    return i;
                }
            }

            return 0;
        }

        //get corresponding weapon selector index from weapon object
        private int GetWeaponIndex ( GameObject weapon)
        {
            if (weapon == null) return 0;
            for ( int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].name == weapon.name) return i;
            }

            return 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G)) OnDropWeapon();
            if (Input.GetKeyDown(KeyCode.K)) OnKillRevive();
            if (Input.GetKeyDown(KeyCode.LeftBracket)) OnInjureFront();
            if (Input.GetKeyDown(KeyCode.RightBracket)) OnInjureBack();
        }
    }
}
