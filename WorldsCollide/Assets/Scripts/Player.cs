using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Animator animator;
    private float direction;
    // public float speed;
    public GameObject weapon;
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    private float timeBtwProject;
    public float startTimeBtwProject;
    public float timeBtwShield;
    public float startTimeBtwShield;
    public Transform attackPos;
    public LayerMask enemyLayer;
    public float meleeRange;
    public int meleeDamage;
    public GameObject bullet;
    public InventoryItemBase meleeWeapon; // Should be Sword
    public InventoryItemBase rangedWeapon;
    public InventoryItemBase equipment;
    public bool blocking;
    public bool attacking;
	public bool playercanmove = true;
    public bool firing;
    public bool staffEquipped;
    public bool shieldEquipped;
    public Transform inventoryPanel;

    private InventoryItemBase mCurrentItem = null;
    public Inventory Inventory;
    public HUD Hud;
    private List<InventoryItemBase> itemsEquipped = new List<InventoryItemBase>(); // Maintains which slots are actually filled.

    void Start()
    {
        SetPlayerStartingItems();
    }

    // Update is called once per frame
    void Update()
    {
        /* Movement */
		if(!playercanmove){
			return;
		}
        UpdatePlayerAnimatorAndPosition();

        /* Detect keys pressed that work every frame */
        if (mInteractItem != null && Input.GetKeyDown(KeyCode.F))
        {
            InteractWithItem();
        }
        AttemptChangeEquipment();

        /* Detect key presses that are conditional based on a timer (Shield, Melee, Projectile) */
        if (timeBtwShield <= 0)
        {
            if(blocking == true){
                animator.SetTrigger("ExitShielding");
                blocking = false;
            }
            AttemptPlayerShield();
        } else
            timeBtwShield -= Time.deltaTime;
        if (timeBtwAttack <= 0){ // If this checks for KeyCode.Space instead, it fails to register sometimes.
            attacking = false;
            AttemptPlayerAttack();
        } else
            timeBtwAttack -= Time.deltaTime;
        if (timeBtwProject <= 0){
            firing = false;
            AttemptPlayerProjectile();
        } else
            timeBtwProject -= Time.deltaTime;
    }

    void SetPlayerStartingItems()
    {
        int count = 0;
        Inventory.ItemUsed += Inventory_ItemUsed;
        Inventory.ItemRemoved += Inventory_ItemRemoved;
        if (meleeWeapon != null)
        {
            InventoryItemBase sword = meleeWeapon.GetComponent<InventoryItemBase>();
            Inventory.AddItem(sword);
            // Update GUI label and add to existing list to minimize iteration on HUD slots later
            UpdateAssignedKeyText(count++, "J");
            itemsEquipped.Add(sword);
        }
        if (rangedWeapon != null)
        {
            InventoryItemBase startRanged = rangedWeapon.GetComponent<InventoryItemBase>();
            UpdateAssignedKeyText(count++, "K");
            Inventory.AddItem(startRanged);
        }
        if (equipment != null)
        {
            InventoryItemBase startEquip = equipment.GetComponent<InventoryItemBase>();
            UpdateAssignedKeyText(count++, "L");
            Inventory.AddItem(startEquip);
        }
        SetPlayerItemBooleans();
    }

    void UpdateAssignedKeyText(int slot, string desiredCharacter)
    {
        Transform keyTextTransform = inventoryPanel.GetChild(slot).GetChild(0).GetChild(2);
        Text assignedKey = keyTextTransform.GetComponent<Text>();
        assignedKey.text = desiredCharacter;
    }

    void SetPlayerItemBooleans()
    {
        if ((rangedWeapon != null) && (rangedWeapon.Name == "Staff"))
            staffEquipped = true;
        else
            staffEquipped = false;

        if ((equipment != null) && (equipment.Name == "Shield"))
            shieldEquipped = true;
        else
            shieldEquipped = false;
    }

    void SetAnimatorVariables(Vector3 movementVector)
    {
		if(!playercanmove){
			return;
		}
        float horz = movementVector.x;
        float vert = movementVector.y;
        direction = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) / Mathf.PI;
        animator.SetFloat("Horizontal", horz);
        animator.SetFloat("Vertical", vert);
        animator.SetFloat("Direction", direction);
    }

    void UpdatePlayerAnimatorAndPosition()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f);
        animator.SetFloat("Magnitude", movement.magnitude);
        if (movement != Vector3.zero) // Avoid player always facing "0" direction when idle
        {
            
			if(!playercanmove){
				return;
			}
			SetAnimatorVariables(movement);
            transform.position = transform.position + movement * Time.deltaTime;
            
            // set melee hitbox direction TODO fix to account for player sprite pivot!  Look at InvisibleSword to see
            float absX = Mathf.Abs(movement.x);
            float absY = Mathf.Abs(movement.y);
            if (absX > absY) // Player is moving more horz than vert
            {
                if (movement.x > 0) // Player is moving right
                {
                    attackPos.position = transform.position + (0.1f * Vector3.right); // Set weapon to be on the right of the player
                } 
                else if (movement.x < 0)
                {
                    attackPos.position = transform.position + (0.1f * Vector3.left); // Set weapon to be on the left of the player
                }
            }
            else // Player is moving more vert than horz
            {
                if (movement.y > 0) 
                {
                    attackPos.position = transform.position + (0.1f * Vector3.up); // Set weapon above player
                }
                else if (movement.y < 0)
                {
                    attackPos.position = transform.position + (0.1f * Vector3.down); 
                }
            }
        }
    }

    void AttemptChangeEquipment()
    {
        int keyPressed = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1) || (Input.GetKeyDown(KeyCode.Keypad1)))
            keyPressed = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2) || (Input.GetKeyDown(KeyCode.Keypad2)))
            keyPressed = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3) || (Input.GetKeyDown(KeyCode.Keypad3)))
            keyPressed = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4) || (Input.GetKeyDown(KeyCode.Keypad4)))
            keyPressed = 4;
        if (Input.GetKeyDown(KeyCode.Alpha5) || (Input.GetKeyDown(KeyCode.Keypad5)))
            keyPressed = 5;
        if (Input.GetKeyDown(KeyCode.Alpha6) || (Input.GetKeyDown(KeyCode.Keypad6)))
            keyPressed = 6;
        if (Input.GetKeyDown(KeyCode.Alpha7) || (Input.GetKeyDown(KeyCode.Keypad7)))
            keyPressed = 7;
        if (Input.GetKeyDown(KeyCode.Alpha8) || (Input.GetKeyDown(KeyCode.Keypad8)))
            keyPressed = 8;
        if (Input.GetKeyDown(KeyCode.Alpha9) || (Input.GetKeyDown(KeyCode.Keypad9)))
            keyPressed = 9;

        // Early exit if no input
        if (keyPressed == 0)
            return;
        Debug.Log("0");

        // If the slot pressed has an item in it which is not already equipped, assert it becomes equipped and the has correct label.
        InventoryItemBase itemFromSlotPressed = Inventory.GetItemFromSlot(keyPressed-1);
        if ((itemFromSlotPressed != null) && (itemFromSlotPressed != meleeWeapon) && (itemFromSlotPressed != rangedWeapon) && (itemFromSlotPressed != equipment))
        {
            Debug.Log("1");
            if (itemFromSlotPressed.ItemType == EItemType.Default) // Shield, etc.
            {
                // Reset the old equipment's label back to its numkey, which happens to be its slot (also must account for index start @ 0)
                UpdateAssignedKeyText((equipment.Slot.Id), (equipment.Slot.Id+1).ToString());
                // Replace the gameobject attached to the player with new equipment
                equipment = itemFromSlotPressed;
                UpdateAssignedKeyText(keyPressed-1, "L");
            }
            else if (itemFromSlotPressed.ItemType == EItemType.RangedWeapon) // Staff, etc.
            {
                Debug.Log("2");
                if (rangedWeapon != null)
                    UpdateAssignedKeyText((rangedWeapon.Slot.Id), (rangedWeapon.Slot.Id+1).ToString());
                rangedWeapon = itemFromSlotPressed;
                UpdateAssignedKeyText(keyPressed-1, "K");
            }
            // if (itemFromSlotPressed.ItemType == EItemType.MeleeWeapon) // Only sword, so will never be used.
            SetPlayerItemBooleans();
        } else if (itemFromSlotPressed == null) {
            Debug.Log("[Empty: Inventory Slot #" + keyPressed + "]");
        }
    }

    void AttemptPlayerShield()
    {
		if(!playercanmove){
			return;
		}
        if(Input.GetKey(KeyCode.L) && (attacking == false) && (firing == false)){
            if (!shieldEquipped)
            {
                Debug.Log("Can't use shield yet, none have been found!");
                return;
            }
            animator.SetTrigger("Shielding");
            blocking = true;
            timeBtwShield = startTimeBtwShield;
        }
        /*
        if(Input.GetKeyUp(KeyCode.X)){
            blocking = false;
        }
        */

    }

    void AttemptPlayerAttack()
    {
		if(!playercanmove){
			return;
		}
        if ((Input.GetKey(KeyCode.J)) && (firing == false))
        {
            // player can attack
            attacking = true;
            if(blocking == true){
                animator.SetTrigger("ExitShielding");
                blocking = false;
            }
            animator.SetTrigger("Attacking");
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, meleeRange, enemyLayer);
            Debug.Log("# enemies found in Player melee hitbox: " + enemiesToDamage.Length);
            for (int i=0; i < enemiesToDamage.Length; i++)
            {
                // damage enemy
                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(meleeDamage);
            }
            timeBtwAttack = startTimeBtwAttack;
        }
    }

    void AttemptPlayerProjectile()
    {
		if(!playercanmove){
			return;
		}
        if ((Input.GetKey(KeyCode.K)) && (attacking == false))
        {
            if (!staffEquipped)
            {
                Debug.Log("Can't use any ranged weapons yet, none have been found!");
                return;
            }
            firing = true;
            if(blocking == true){
                animator.SetTrigger("ExitShielding");
                blocking = false;
            }
            animator.SetTrigger("CastingFireball");

            // Rotate bullet sprite to go with player direction
            Quaternion fixedDirection = Quaternion.identity;
            fixedDirection.eulerAngles = new Vector3(0, 0, 180 - (direction * 180)); // Multiply by 180 to convert to degrees.
            Debug.Log("direction = " + direction);

            Vector2 projectilePosition = transform.position;
            float projectileAngle = Mathf.Round(direction * 2) / 2; // Round to nearest 0.5
            if (projectileAngle == -0.5f) // if facing left
            {
                projectilePosition.x -= 0.15f; // offset from player pivot (to avoid shooting from chest)
                projectilePosition.y += 0.15f; // offset from player feet pivot
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                go.GetComponent<ProjectileController>().SetProjectileVector("Left");
            }
            else if (projectileAngle == 0) // if facing up
            {
                projectilePosition.y += 0.15f;
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                go.GetComponent<ProjectileController>().SetProjectileVector("Up");
            }
            else if (projectileAngle == 0.5f) // if facing right
            {
                projectilePosition.x += 0.15f;
                projectilePosition.y += 0.15f;
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                go.GetComponent<ProjectileController>().SetProjectileVector("Right");
                
            }
            else if ((projectileAngle == 1) || (projectileAngle == -1)) // if facing down
            {
                projectilePosition.y -= 0.15f;
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                go.GetComponent<ProjectileController>().SetProjectileVector("Down");
            }
            // We don't want the player to shoot diagonally.
            // else if (projectileAngle == -0.25) // if facing up-left
            // {
            //     projectilePosition.y += 0.15f;
            //     projectilePosition.x -= 0.15f;
            //     GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
            //     go.GetComponent<ProjectileController>().SetProjectileVector("UpLeft");
            // }
            // else if (projectileAngle == 0.25) // if facing up-right
            // {
            //     projectilePosition.y += 0.15f;
            //     projectilePosition.x += 0.15f;
            //     GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
            //     go.GetComponent<ProjectileController>().SetProjectileVector("UpRight");
            // }
            // else if (projectileAngle == 0.75) // if facing down-right
            // {
            //     projectilePosition.y -= 0.15f;
            //     projectilePosition.x += 0.15f;
            //     GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
            //     go.GetComponent<ProjectileController>().SetProjectileVector("DownRight");
            // }
            // else if (projectileAngle == -0.75f) // if facing down-left
            // {
            //     projectilePosition.y -= 0.15f;
            //     projectilePosition.x -= 0.15f;
            //     GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
            //     go.GetComponent<ProjectileController>().SetProjectileVector("DownLeft");
            // }
            timeBtwProject = startTimeBtwProject;
        }
    }

    /* INVENTORY */
    private void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
    {
        InventoryItemBase item = e.Item;

        GameObject goItem = (item as MonoBehaviour).gameObject;
        goItem.SetActive(true);
    }

    private void SetItemActive(InventoryItemBase item, bool active)
    {
        GameObject currentItem = (item as MonoBehaviour).gameObject;
        currentItem.SetActive(active);
    }

    private void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        if (e.Item.ItemType != EItemType.Consumable)
        {
            // If the player carries an item, un-use it (remove from player's hand)
            if (mCurrentItem != null)
            {
                SetItemActive(mCurrentItem, false);
            }

            InventoryItemBase item = e.Item;

            // Use item (put it to hand of the player)
            SetItemActive(item, true);

            mCurrentItem = e.Item;
        }

    }

    public void InteractWithItem()
    {
        if (mInteractItem != null)
        {
            mInteractItem.OnInteract();

            if (mInteractItem is InventoryItemBase)
            {
                InventoryItemBase inventoryItem = mInteractItem as InventoryItemBase;
                Inventory.AddItem(inventoryItem);
                inventoryItem.OnPickup();

                if (inventoryItem.UseItemAfterPickup)
                {
                    Inventory.UseItem(inventoryItem);
                }
            }
        }

        Hud.CloseMessagePanel();

        mInteractItem = null;
    }

    private InventoryItemBase mInteractItem = null;

    /* END INVENTORY */

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, meleeRange);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        InventoryItemBase item = other.GetComponent<InventoryItemBase>();

        if (item != null)
        {
            if (item.CanInteract(other))
            {
                mInteractItem = item;
                Hud.OpenMessagePanel(mInteractItem);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        InteractableItemBase item = other.GetComponent<InteractableItemBase>();
        if (item != null)
        {
            Hud.CloseMessagePanel();
            mInteractItem = null;
        }
    }
}
