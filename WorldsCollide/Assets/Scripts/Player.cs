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
    public float timeBtwEquipment;
    public float startTimeBtwEquipment;
    public float timeBtwShield;
    public float startTimeBtwShield;
    public float timeBtwSoda;
    public float startTimeBtwSoda;
    public float timeBtwStopwatch;
    public float startTimeBtwStopwatch;
    public float timeBtwFireball;
    public float startTimeBtwFireball;
    public float timeBtwGrenade;
    public float startTimeBtwGrenade;
    public float timeBtwShotgun;
    public float startTimeBtwShotgun;
    public float timeBtwLaser;
    public float startTimeBtwLaser;
    public Transform attackPos;
    public LayerMask enemyLayer;
    public float meleeRange;
    public int meleeDamage;
    public Slider stop_time_count;
    public GameObject bullet;
    public GameObject shotgunPellet;
    public GameObject grenade;
    public InventoryItemBase meleeWeapon; // Should be Sword
    public InventoryItemBase rangedWeapon;
    public InventoryItemBase equipment;
    public bool blocking;
    public bool attacking;
	public bool playercanmove = true;
    public bool firing;
    public bool staffEquipped;
    public bool shieldEquipped;
    public bool time_stopped;
    public int start_time_stop_counter = 300;
    public int time_stop_counter = 0;
    public bool is_moving;
    public Transform inventoryPanel;
    AudioSource playerAudio;
    public AudioClip fireballClip;
    public AudioClip shotgunClip;
    public AudioClip meleeClip;
    public AudioClip shieldClip;
    public AudioClip timeStopWatchClip;
    public AudioClip nukaSodaClip;
    private Rigidbody2D rb2d;
    
    private InventoryItemBase mCurrentItem = null;
    public Inventory Inventory;
    public HUD Hud;
    private List<InventoryItemBase> itemsEquipped = new List<InventoryItemBase>(); // Maintains which slots are actually filled.

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        SetPlayerStartingItems();
        if (stop_time_count != null)
            stop_time_count.gameObject.SetActive(false);
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = Vector2.zero;
        /* Movement */
		if (!playercanmove)
			return;

        UpdatePlayerAnimatorAndPosition();


        /* Detect key presses that work every frame */
        if (mInteractItem != null && Input.GetKeyDown(KeyCode.F))
            InteractWithItem();

        AttemptChangeEquipment();


        /* Detect key presses that are conditional based on a timer (Shield, Melee, Projectile) */
        if (timeBtwEquipment <= 0)
        {
            if (blocking == true)
            {
                animator.SetTrigger("ExitShielding");
                blocking = false;
            }
            AttemptPlayerEquipment();
        } else
            timeBtwEquipment -= Time.deltaTime;

        if (time_stopped){
            if (time_stop_counter <= 0){
                time_stopped = false;
                timeBtwEquipment = 0;
                stop_time_count.gameObject.SetActive(false);
            }
            else
            {
                stop_time_count.value = time_stop_counter;

            }
            
        }

        if (timeBtwAttack <= 0) // If this checks for KeyCode.Space instead, it fails to register sometimes.
        { 
            attacking = false;
            AttemptPlayerAttack();
        } else
            timeBtwAttack -= Time.deltaTime;

        if (timeBtwProject <= 0)
        {
            firing = false;
            AttemptPlayerProjectile();
        } else
            timeBtwProject -= Time.deltaTime;
    }

    void SetPlayerStartingItems()
    {
        int count = 0;
        if (Inventory != null)
        {
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

            // if (rangedWeapon != null)
            // {
            //     InventoryItemBase startRanged = rangedWeapon.GetComponent<InventoryItemBase>();
            //     UpdateAssignedKeyText(count++, "K");
            //     Inventory.AddItem(startRanged);
            // }

            // if (equipment != null)
            // {
            //     InventoryItemBase startEquip = equipment.GetComponent<InventoryItemBase>();
            //     UpdateAssignedKeyText(count++, "L");
            //     Inventory.AddItem(startEquip);
            // }

            if (GameController.control.grenade)
            {
                InventoryItemBase grenadeInventoryObject = GameObject.FindGameObjectWithTag("PlayerGrenade").GetComponent<Staff>();
                Inventory.AddItem(grenadeInventoryObject);
                itemsEquipped.Add(grenadeInventoryObject);
            }
            if (GameController.control.shield)
            {
                InventoryItemBase shieldInventoryObject = GameObject.FindGameObjectWithTag("PlayerShield").GetComponent<Staff>();
                Inventory.AddItem(shieldInventoryObject);
                itemsEquipped.Add(shieldInventoryObject);
            }
            if (GameController.control.shotgun)
            {
                InventoryItemBase shotgunInventoryObject = GameObject.FindGameObjectWithTag("PlayerShotgun").GetComponent<Staff>();
                Inventory.AddItem(shotgunInventoryObject);
                itemsEquipped.Add(shotgunInventoryObject);
            }
            if (GameController.control.staff)
            {
                InventoryItemBase staffInventoryObject = GameObject.FindGameObjectWithTag("PlayerStaff").GetComponent<Staff>();
                Inventory.AddItem(staffInventoryObject);
                itemsEquipped.Add(staffInventoryObject);
            }
            if (GameController.control.watch)
            {
                InventoryItemBase watchInventoryObject = GameObject.FindGameObjectWithTag("PlayerWatch").GetComponent<Staff>();
                Inventory.AddItem(watchInventoryObject);
                itemsEquipped.Add(watchInventoryObject);
            }
            if (GameController.control.soda)
            {
                InventoryItemBase sodaInventoryObject = GameObject.FindGameObjectWithTag("PlayerSoda").GetComponent<Staff>();
                Inventory.AddItem(sodaInventoryObject);
                itemsEquipped.Add(sodaInventoryObject);
            }
            // if (GameController.control.cannon)
            // {
            //     InventoryItemBase cannonInventoryObject = GameObject.FindGameObjectWithTag("PlayerCannon").GetComponent<Staff>();
            //     Inventory.AddItem(cannonInventoryObject);
            //     itemsEquipped.Add(cannonInventoryObject);
            // }

            SetPlayerItemBooleans();
        }
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
		if (!playercanmove)
			return;
        
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
			if (!playercanmove)
				return;
            is_moving = true;
            if (time_stopped){
                time_stop_counter-=1;
            }

			SetAnimatorVariables(movement);
            transform.position = transform.position + movement * Time.deltaTime;
            
            // Set melee hitbox direction
            float absX = Mathf.Abs(movement.x);
            float absY = Mathf.Abs(movement.y);
            if (absX > absY) // Player is moving more horz than vert
            {
                if (movement.x > 0) // Player is moving right
                    attackPos.position = transform.position + (0.15f * Vector3.right) + (0.1f * Vector3.up);
                else if (movement.x < 0)
                    attackPos.position = transform.position + (0.15f * Vector3.left) + (0.1f * Vector3.up);
            } else // Player is moving more vert than horz
            {
                if (movement.y > 0) 
                    attackPos.position = transform.position + (0.25f * Vector3.up);
                else if (movement.y < 0)
                    attackPos.position = transform.position; 
            }
        }
        else{
            is_moving = false;
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

        // If the slot pressed has an item in it which is not already equipped, assert it becomes equipped and the has correct label.
        InventoryItemBase itemFromSlotPressed = Inventory.GetItemFromSlot(keyPressed-1);
        if ((itemFromSlotPressed != null) && (itemFromSlotPressed != meleeWeapon) && (itemFromSlotPressed != rangedWeapon) && (itemFromSlotPressed != equipment))
        {
            if (itemFromSlotPressed.ItemType == EItemType.Default) // Shield, etc.
            {
                if (equipment != null)
                    // Reset the old equipment's label back to its numkey, which happens to be its slot (also must account for index start @ 0)
                    UpdateAssignedKeyText((equipment.Slot.Id), (equipment.Slot.Id+1).ToString());
                
                // Replace the gameobject attached to the player with the new equipment
                equipment = itemFromSlotPressed;
                UpdateAssignedKeyText(keyPressed-1, "L");
            } else if (itemFromSlotPressed.ItemType == EItemType.RangedWeapon) // Staff, etc.
            {
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

    void AttemptPlayerEquipment()
    {
		if (!playercanmove)
			return;

        if (Input.GetKey(KeyCode.L) && (attacking == false) && (firing == false))
        {
            if (equipment == null)
            {
                Debug.Log("Can't use equipment yet, none have been found!");
                return;
            } else
            {
                if (equipment.GetComponent<Staff>().Name.Equals("Shield"))
                {
                    startTimeBtwEquipment = 10.0f;
                    animator.SetTrigger("Shielding");
                    playerAudio.clip = shieldClip;
                    playerAudio.Play();
                    blocking = true;
                }
                else if (equipment.GetComponent<Staff>().Name.Equals("Time Stop Watch"))
                {
                    startTimeBtwEquipment = 5.0f;
                    Debug.Log("Stop time!");
                    time_stopped = true;
                    time_stop_counter = start_time_stop_counter;
                    playerAudio.clip = timeStopWatchClip;
                    stop_time_count.gameObject.SetActive(true);
                    //gameObject.GetComponent<Time_Stop_Effect>;
                    playerAudio.Play();
                    // 1. Freeze enemies, 2. add blue tint to screen (perhaps copy damageImage from PlayerHealth?)
                }
                else if (equipment.GetComponent<Staff>().Name.Equals("Nuka Soda"))
                {
                    startTimeBtwEquipment = 15.0f;
                    Debug.Log("Buff player!");
                    playerAudio.clip = nukaSodaClip;
                    playerAudio.Play();
                    // 1. Increase player movement speed, 2. Increase melee attack speed [3. Decrease cooldowns?]
                }
            }
            timeBtwEquipment = startTimeBtwEquipment;
        }
    }

    void AttemptPlayerAttack()
    {
		if (!playercanmove)
			return;

        if ((Input.GetKey(KeyCode.J)) && (firing == false))
        {
            // player can attack
            attacking = true;
            if (blocking == true)
            {
                animator.SetTrigger("ExitShielding");
                blocking = false;
                if (shieldEquipped){
                    timeBtwEquipment = 1f;
                }
            }

            if (time_stopped){
                time_stop_counter-=200;
            }

            animator.SetTrigger("Attacking");
            playerAudio.clip = meleeClip;
            playerAudio.Play();
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
		if (!playercanmove)
			return;
        if ((Input.GetKey(KeyCode.K)) && (attacking == false))
        {
            if (rangedWeapon == null)
            {
                Debug.Log("Can't use any ranged weapons yet, none have been found!");
                return;
            } else 
            {
                if (rangedWeapon.GetComponent<Staff>().Name.Equals("Staff"))
                {
                    animator.SetTrigger("CastingFireball");
                    playerAudio.clip = fireballClip;
                    playerAudio.Play();

                    if (time_stopped){
                        time_stop_counter-=200;
                    }       

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
                        go.GetComponent<Rigidbody2D>().velocity = Vector2.left;
                    }
                    else if (projectileAngle == 0) // if facing up
                    {
                        projectilePosition.y += 0.25f;
                        GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                        go.GetComponent<Rigidbody2D>().velocity = Vector2.up;
                    }
                    else if (projectileAngle == 0.5f) // if facing right
                    {
                        projectilePosition.x += 0.15f;
                        projectilePosition.y += 0.15f;
                        GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                        go.GetComponent<Rigidbody2D>().velocity = Vector2.right;
                        
                    }
                    else if ((projectileAngle == 1) || (projectileAngle == -1)) // if facing down
                    {
                        GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                        go.GetComponent<Rigidbody2D>().velocity = Vector2.down;
                    }
                }
                else if (rangedWeapon.GetComponent<Staff>().Name.Equals("Shotgun"))
                {
                    animator.SetTrigger("ShotgunShot");
                    playerAudio.clip = shotgunClip;
                    playerAudio.Play();
                    if (time_stopped){
                        time_stop_counter-=200;
                    } 
                    for (int i=0; i<3; i++)
                    {
                        // Spawns bullet
                        GameObject tempBullet = null;
                        // Randomize angle variation between bullets
                        float spreadAngle = Random.Range(-10, 10);
                        float rotateAngle = 0;
                        // Rotate bullet sprite to go with player direction
                        Quaternion fixedDirection = Quaternion.identity;
                        fixedDirection.eulerAngles = new Vector3(0, 0, 180 - (direction * 180)); // Multiply by 180 to convert to degrees.

                        Vector2 projectilePosition = transform.position;
                        float projectileAngle = Mathf.Round(direction * 2) / 2; // Round to nearest 0.5
                        if (projectileAngle == -0.5f) // if facing left
                        {
                            projectilePosition.x -= 0.15f; // offset from player pivot (to avoid shooting from chest)
                            projectilePosition.y += 0.15f; // offset from player feet pivot
                            tempBullet = (GameObject)Instantiate(shotgunPellet, projectilePosition, fixedDirection);
                            rotateAngle = spreadAngle + (Mathf.Atan2(Vector2.left.y, Vector2.left.x) * Mathf.Rad2Deg);
                        }
                        else if (projectileAngle == 0) // if facing up
                        {
                            projectilePosition.y += 0.25f;
                            tempBullet = (GameObject)Instantiate(shotgunPellet, projectilePosition, fixedDirection);
                            rotateAngle = spreadAngle + (Mathf.Atan2(Vector2.up.y, Vector2.up.x) * Mathf.Rad2Deg);
                        }
                        else if (projectileAngle == 0.5f) // if facing right
                        {
                            projectilePosition.x += 0.15f;
                            projectilePosition.y += 0.15f;
                            tempBullet = (GameObject)Instantiate(shotgunPellet, projectilePosition, fixedDirection);
                            rotateAngle = spreadAngle + (Mathf.Atan2(Vector2.right.y, Vector2.right.x) * Mathf.Rad2Deg);
                            
                        }
                        else if ((projectileAngle == 1) || (projectileAngle == -1)) // if facing down
                        {
                            tempBullet = (GameObject)Instantiate(shotgunPellet, projectilePosition, fixedDirection);
                            rotateAngle = spreadAngle + (Mathf.Atan2(Vector2.down.y, Vector2.down.x) * Mathf.Rad2Deg);
                        }

                        // Calculate the new direction we will move in which takes into account 
                        // the random angle generated
                        var MovementDirection = new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad)).normalized;

                        if (tempBullet != null)
                        {
                            Rigidbody2D tempBulletRB = tempBullet.GetComponent<Rigidbody2D>();
                            // tempBulletRB.velocity = MovementDirection * bulletSpeed;
                            tempBulletRB.velocity = MovementDirection * 5;
                            Destroy(tempBullet, 0.3f);
                        }
                    }
                }
                else if (rangedWeapon.GetComponent<Staff>().Name.Equals("Holy Hand Grenade"))
                {
                    if (time_stopped){
                        time_stop_counter-=400;
                    }
                    // Rotate grenade sprite to go with player direction
                    Quaternion fixedDirection = Quaternion.identity;
                    fixedDirection.eulerAngles = new Vector3(0, 0, 180 - (direction * 180)); // Multiply by 180 to convert to degrees.
                    Debug.Log("direction = " + direction);

                    Vector2 projectilePosition = transform.position;
                    float projectileAngle = Mathf.Round(direction * 2) / 2; // Round to nearest 0.5
                    if (projectileAngle == -0.5f) // if facing left
                    {
                        projectilePosition.x -= 0.15f; // offset from player pivot (to avoid shooting from chest)
                        projectilePosition.y += 0.15f; // offset from player feet pivot
                        GameObject go = (GameObject)Instantiate (grenade, projectilePosition, fixedDirection);
                        go.GetComponent<Rigidbody2D>().velocity = Vector2.left;
                    }
                    else if (projectileAngle == 0) // if facing up
                    {
                        projectilePosition.y += 0.25f;
                        GameObject go = (GameObject)Instantiate (grenade, projectilePosition, fixedDirection);
                        go.GetComponent<Rigidbody2D>().velocity = Vector2.up;
                    }
                    else if (projectileAngle == 0.5f) // if facing right
                    {
                        projectilePosition.x += 0.15f;
                        projectilePosition.y += 0.15f;
                        GameObject go = (GameObject)Instantiate (grenade, projectilePosition, fixedDirection);
                        go.GetComponent<Rigidbody2D>().velocity = Vector2.right;
                        
                    }
                    else if ((projectileAngle == 1) || (projectileAngle == -1)) // if facing down
                    {
                        GameObject go = (GameObject)Instantiate (grenade, projectilePosition, fixedDirection);
                        go.GetComponent<Rigidbody2D>().velocity = Vector2.down;
                    }
                }
            }

            firing = true;
            if (blocking == true)
            {
                animator.SetTrigger("ExitShielding");
                blocking = false;
                if (shieldEquipped){
                    timeBtwEquipment = 1f;
                }
            }
            
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

		if(other.name == "Grenade")
			GameController.control.grenade = true;
		if(other.name == "Shotgun")
			GameController.control.shotgun = true;
		if(other.name == "Shield")
			GameController.control.shield = true;
		if(other.name == "Staff")
			GameController.control.staff = true;
		if(other.name == "TimeStopWatch")
			GameController.control.watch = true;
		if(other.name == "NukaSoda")
			GameController.control.soda = true;
		if(other.name == "LaserCannon")
			GameController.control.cannon = true;



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
