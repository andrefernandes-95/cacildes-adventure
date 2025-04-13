namespace AF
{
    using System.Collections.Generic;
    using UnityEngine;
    using AYellowpaper.SerializedCollections;

    public class SyntyCharacterModelManager : MonoBehaviour
    {
        public SerializedDictionary<string, GameObject> syntyCharacterBodyParts = new();

        public CharacterBaseManager character;

        public void Initialize()
        {
            CacheCharacterParts();
            InitializeDefaultBodyParts();
        }

        void InitializeDefaultBodyParts()
        {
            EnableHair();
            EnableEyebrows();
            EnableBeard();
            EnableFace();
            EnableTorso();
            EnableHands();
            EnableLegs();
        }

        void CacheCharacterParts()
        {
            syntyCharacterBodyParts.Clear();

            var syntyPieces = transform.GetChild(0).GetComponentsInChildren<Transform>(true);

            foreach (Transform t in syntyPieces)
            {
                if (!syntyCharacterBodyParts.ContainsKey(t.gameObject.name))
                {
                    syntyCharacterBodyParts.Add(t.gameObject.name, t.gameObject);

                    // Hide All Pieces By Default
                    t.gameObject.SetActive(false);
                }
            }

            if (syntyCharacterBodyParts.ContainsKey("Armature"))
            {
                // TODO: Improve this logic
                syntyCharacterBodyParts["Armature"].SetActive(true);
                foreach (Transform child in syntyCharacterBodyParts["Armature"].transform)
                {
                    SetActiveRecursively(child.gameObject, true);
                }
            }

            if (syntyCharacterBodyParts.ContainsKey("Exported Synty Character"))
            {
                syntyCharacterBodyParts["Exported Synty Character"].SetActive(true);
            }
        }
        void SetActiveRecursively(GameObject obj, bool isActive)
        {
            obj.SetActive(isActive);

            foreach (Transform child in obj.transform)
            {
                SetActiveRecursively(child.gameObject, isActive);
            }
        }

        #region Hair
        public void EnableHair()
        {
            EnablePieces(character.characterBaseAppearance.GetHairs(),
                         updateHairMaterials: true,
                         updateSkinMaterials: false,
                         updateEyeMaterials: false,
                         updateStubbleMaterials: false,
                         updateBodyArtMaterials: false);
        }

        public void DisableHair()
        {
            DisablePieces(character.characterBaseAppearance.GetHairs());
        }
        #endregion

        #region Eyebrows
        public void EnableEyebrows()
        {
            EnablePieces(character.characterBaseAppearance.GetEyebrows(),
                         updateHairMaterials: true,
                         updateSkinMaterials: false,
                         updateEyeMaterials: false,
                         updateStubbleMaterials: false,
                         updateBodyArtMaterials: false);
        }

        public void DisableEyebrows()
        {
            DisablePieces(character.characterBaseAppearance.GetEyebrows());
        }
        #endregion

        #region Beards
        public void EnableBeard()
        {
            EnablePieces(character.characterBaseAppearance.GetBeard(),
                         updateHairMaterials: true,
                         updateSkinMaterials: false,
                         updateEyeMaterials: false,
                         updateStubbleMaterials: false,
                         updateBodyArtMaterials: false);
        }

        public void DisableBeard()
        {
            DisablePieces(character.characterBaseAppearance.GetBeard());
        }
        #endregion

        #region Face
        public void EnableFace()
        {
            EnablePieces(character.characterBaseAppearance.GetFace(),
                         updateHairMaterials: false,
                         updateSkinMaterials: true,
                         updateEyeMaterials: true,
                         updateStubbleMaterials: true,
                         updateBodyArtMaterials: true);
        }

        public void DisableFace()
        {
            DisablePieces(character.characterBaseAppearance.GetFace());
        }
        #endregion

        #region Torso
        public void EnableTorso()
        {
            EnablePieces(character.characterBaseAppearance.GetTorso(),
                         updateHairMaterials: false,
                         updateSkinMaterials: true,
                         updateEyeMaterials: false,
                         updateStubbleMaterials: false,
                         updateBodyArtMaterials: false);
        }

        public void DisableTorso()
        {
            DisablePieces(character.characterBaseAppearance.GetTorso());
        }
        #endregion

        #region Hands
        public void EnableHands()
        {
            EnablePieces(character.characterBaseAppearance.GetHands(),
                         updateHairMaterials: false,
                         updateSkinMaterials: true,
                         updateEyeMaterials: false,
                         updateStubbleMaterials: false,
                         updateBodyArtMaterials: false);
        }

        public void DisableHands()
        {
            DisablePieces(character.characterBaseAppearance.GetHands());
        }
        #endregion

        #region Legs
        public void EnableLegs()
        {
            EnablePieces(character.characterBaseAppearance.GetLegs(),
                         updateHairMaterials: false,
                         updateSkinMaterials: true,
                         updateEyeMaterials: false,
                         updateStubbleMaterials: false,
                         updateBodyArtMaterials: false);
        }

        public void DisableLegs()
        {
            DisablePieces(character.characterBaseAppearance.GetLegs());
        }
        #endregion


        void EnablePieces(
            List<string> pieces,
            bool updateHairMaterials,
            bool updateSkinMaterials,
            bool updateEyeMaterials,
            bool updateStubbleMaterials,
            bool updateBodyArtMaterials
        )
        {
            foreach (string gameObjectName in pieces)
            {
                if (!syntyCharacterBodyParts.ContainsKey(gameObjectName))
                {
                    continue;
                }

                GameObject bodyPieces = syntyCharacterBodyParts[gameObjectName];

                // Set Materials
                if (bodyPieces.TryGetComponent(out SkinnedMeshRenderer skinnedMeshRenderer))
                {
                    var originalMaterials = skinnedMeshRenderer.materials;

                    List<Material> newMaterials = new();

                    foreach (var originalMaterial in originalMaterials)
                    {
                        // Clone the material to avoid modifying the shared one
                        Material clonedMaterial = new Material(originalMaterial);

                        // Apply color changes
                        if (updateHairMaterials)
                        {
                            clonedMaterial.SetColor("_Color_Hair", character.characterBaseAppearance.GetHairColor());
                        }

                        if (updateSkinMaterials)
                        {
                            clonedMaterial.SetColor("_Color_Skin", character.characterBaseAppearance.GetSkinColor());
                        }

                        if (updateStubbleMaterials)
                        {
                            clonedMaterial.SetColor("_Color_Stubble", character.characterBaseAppearance.GetSkinColor());
                        }

                        if (updateEyeMaterials)
                        {
                            clonedMaterial.SetColor("_Color_Eyes", character.characterBaseAppearance.GetEyeColor());
                        }

                        if (updateBodyArtMaterials)
                        {
                            clonedMaterial.SetColor("_Color_BodyArt", character.characterBaseAppearance.GetScarColor());
                        }

                        newMaterials.Add(clonedMaterial);
                    }

                    // Set the new cloned materials back to the renderer
                    skinnedMeshRenderer.materials = newMaterials.ToArray();
                }

                // Enable body piece
                bodyPieces.SetActive(true);
            }
        }

        public void DisablePieces(List<string> pieces)
        {
            foreach (string gameObjectName in pieces)
            {
                if (syntyCharacterBodyParts.ContainsKey(gameObjectName))
                {
                    syntyCharacterBodyParts[gameObjectName].SetActive(false);
                }
            }
        }


        public void EnableArmorPiece(
            List<string> pieces,
            Material armorMaterial
        )
        {
            foreach (string gameObjectName in pieces)
            {
                if (!syntyCharacterBodyParts.ContainsKey(gameObjectName))
                {
                    continue;
                }

                GameObject bodyPieces = syntyCharacterBodyParts[gameObjectName];

                // Set Materials
                if (bodyPieces.TryGetComponent(out SkinnedMeshRenderer skinnedMeshRenderer))
                {

                    // Clone the material to avoid modifying the shared one
                    Material clonedMaterial = new Material(armorMaterial);

                    clonedMaterial.SetColor("_Color_Hair", character.characterBaseAppearance.GetHairColor());
                    clonedMaterial.SetColor("_Color_Skin", character.characterBaseAppearance.GetSkinColor());
                    clonedMaterial.SetColor("_Color_Stubble", character.characterBaseAppearance.GetSkinColor());
                    clonedMaterial.SetColor("_Color_Eyes", character.characterBaseAppearance.GetEyeColor());
                    clonedMaterial.SetColor("_Color_BodyArt", character.characterBaseAppearance.GetScarColor());

                    // Set the new cloned materials back to the renderer
                    skinnedMeshRenderer.material = clonedMaterial;
                }

                // Enable body piece
                bodyPieces.SetActive(true);
            }
        }
    }
}
