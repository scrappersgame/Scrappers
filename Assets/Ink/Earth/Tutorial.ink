/*Location: Earth, but it's basically just a giant landfill. No one lives here if they can avoid it.
Plot: Your handler (Hal the Halibut, looks like robotic Akbar) has you scrapping junk to repay the debt of buying your own personal hand scrapper on credit. You accidentally scrap a drone belonging to the FAT-E’s and they try to kill you to get the scrap back. You end up stealing a FAT-E ship and head out into space. 

Scene opens in a run-down hut with a dirty bed in a corner, exposed pipes, and junk in the corner. A person with a gas mask is holding a scrapper and looking at a distorted projection of what appears to be a robot fish man.
*/

VAR name = "Dave"
TODO get name from save data
Handler: Hey what’s yur name, kid? Can’t work with someone if’n I don’t know their name first…
-> END



=== name_given ===
Hal: Nice to meet ya, {name}. I’m Hal, I’ll be your handler this evenin’, gonna be keepin’ tracka ya to make sure you don’t be runnin’ off with that fairly expensive piece of hardware you got on your back. 
Hal: Now let’s get you out there scrappin’ so you can start payin’ off summa that debt you owe ol’ grouchy britches. Head on outside and I’ll help you find the good bits.
* Wait, how[?] will you know what the good bits are? Are you... spying on me?
    Hal: O'course I'm watchin' ya, how else will I know if you met yur quota? That thing on yur back's got a full spectrum scanner in it, I can see more about what's goin on 'round ya than you can.
        * * Who are you?[] Where am I? What the hell is going on?
            Hal: You get hit in the head or sumthin kid? I'm your scrappin' handler. Now lets go before I scrap you and throw away the blueprint...
                * * * Alright, lets go.
                    ->END
        * * Alright, lets go.
            ->END
* Who are you?[] Where am I? What the hell is going on?
    Hal: You get hit in the head or sumthin kid? I'm your scrappin' handler. Now lets go before I scrap you and throw away the blueprint...
        * * Wait, how[?] will you know what the good bits are? Are you... spying on me?
            Hal: O'course I'm watchin' ya, how else will I know if you met yur quota? That thing on yur back's got a full spectrum scanner in it, I can see more about what's goin on 'round ya than you can.
                * * * Alright, lets go.
                    ->END
        * * Alright, lets go.
            ->END
* Alright, lets go.
    ->END

=== leave_hut
// start dialog after leaving hut.
Hal: Welp, looks like mosta the smaller bits have already been scrapped up by others. Looks like you got your work cut out for ya, kid. There's a pipe over there you can use to whack stuff with. Pick it up and give it a swing.

->END

=== give_gun
Hal: Ya sure are workin up a sweat swinging that pipe around. Head back to the start and you'll find something in your body scrapper that should make the going a bit quicker.
-> END

=== enter_building
Hal: I don't think you're supposed to be in here, boss. I just got an APB from the FAT-Es with your decription, so you might wanna watch out for their drones.

->END

=== ship_intro
???: \\ ERROR \\ CRITICAL DAMAGE \\ REPAIR REQUIRED \\ INPUT REQUIRED SCRAP TO CONTINUE
* Who are you?[] Who's speaking right now?
    PeeCee: Oh, sorry for yelling earlier, I'm used to people just ignoring me. I am PeeCee, the integrated AI for the ship you are currently standing in.
    * * How[?] do I input scrap?
        PeeCee: If you have a device that you can use to project scrap in the direction of my hull, I can handle the integration from there.
* How[?] do I input scrap?
    ???: If you have a device that you can use to project scrap in the direction of my hull, I can handle the integration from there.

->END
