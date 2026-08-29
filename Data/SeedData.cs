using ChurchOfPantoufle.Models;

namespace ChurchOfPantoufle.Data;

public static class SeedData
{
    public static void Initialize(ChurchContext context)
    {
        context.Database.EnsureCreated();

        if (context.Testimonials.Any())
            return;

        context.Testimonials.AddRange(
            new Testimonial
            {
                AuthorName = "Brother Clover",
                WarrenName = "Converted Kit",
                Content = "I was lost in a burrow of despair until The Luminous Fur thumped into my life. Now I leap each morning with purpose and my hay has never tasted sweeter. Praise The Radiant One!",
                DateSubmitted = new DateTime(2025, 3, 15),
                IsApproved = true
            },
            new Testimonial
            {
                AuthorName = "Sister Thistle",
                WarrenName = "Elder Doe",
                Content = "The Great Thumper healed my sore hocks and showed me the true warren. I had wandered from burrow to burrow seeking meaning, but it was His gentle nose-boop upon my forehead that finally brought me peace. May The Velveteen Sovereign reign eternal.",
                DateSubmitted = new DateTime(2025, 6, 22),
                IsApproved = true
            },
            new Testimonial
            {
                AuthorName = "Deacon Bramble",
                WarrenName = "Sire of the Northern Litter",
                Content = "I witnessed The Cottontail Messiah leap across a frozen lake at midnight. The ice did not crack. The stars themselves seemed to thump in rhythm. I fell to my knees and wept emerald-green tears of pure devotion.",
                DateSubmitted = new DateTime(2025, 9, 1),
                IsApproved = true
            },
            new Testimonial
            {
                AuthorName = "Novice Fern",
                WarrenName = "Kit of the Western Rabbitry",
                Content = "Before I found the Warren, I lived in spiritual darkness — eating oatmeal squares without gratitude, thumping without intention. The Omnipotent Oryctolagus opened my eyes. Now every leap is a prayer, every shedding a rebirth. I am forever kindled.",
                DateSubmitted = new DateTime(2025, 11, 10),
                IsApproved = true
            }
        );

        context.SaveChanges();
    }
}
