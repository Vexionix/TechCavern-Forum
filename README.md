# 💬 TechCavern Forum

## 🚀 About Me

I am a 3rd year student at UAIC FII and my passions are coding and gaming. I had contact with technology since I was a kid and that caused this passion to spark.

# Introduction

The project I chose is a forum with tech-related categories that has the purpose to be a place where people can interact with each other on different subjects, learn something new, ask questions and find answers.

I found this project as a great way to learn, with various technologies tackled in the process of development.

To come up with this name I tried to imagine something catchy, yet reserved and as least used as possible and I thought of this, and I decided it is exactly what I wanted.

The thing that kept me the most motivated the whole way through was the fact that it was a passion project and every progress I did brought me joy.

# 👨‍💻 Tech stack

Here's a brief overview of the tech stack:

- This project uses the .NET 8.0 Framework for the BackEnd
- For persistent storage (database), the app uses EFCore
- Hosting the sqlServer is done with the help of Docker
- The FrontEnd is done with React (Vite) in TypeScript
- The design is done using css, the theme of the site is dark and the main font is "Poppins"

# Features

## 🔒 Authentication and session management

The application uses JWT (Json Web Tokens) for the authentication process, storing the user's role and id, with a lifespan of 15 minutes. In order to make this work properly I use refresh tokens stored in the database with a life span of 30 days, so when the JWT expires the user can refresh it with a valid refresh token. The refresh is done when the user gets 401 on a request to the backend in the frontend, then the api route for refresh is called, and if the refresh is successful the initial request(s) are resent with the new JWT in place, otherwise the user gets logged out.

## 🛠️ API Routes

The API is stateless and RESTful and it implements all the methods (get, post, put, patch, delete) in order to interact with the backend.

## 🛡️ Protected routes

Some routes require administrative privileges in order to access them, in case the user tries to circumvent this restriction they are not allowed to the resources and redirected to a 403 Forbidden Page

## 📍🗺️ Routing

Routing is done using React Router Dom and subsequent redirects are mostly done with Link and navigate to provide a seamless experience for the users avoiding useless refreshes of the page when it's not needed.

## 💢 Custom page for invalid routes

There is a custom 404 Not Found page in case the user tries to navigate to an invalid page.

## ⭐ Loading icons from react-icons

The database fields only store the icon name such as "GiIcon" and the resource is loaded from the react-icon package. (Used for categories and subcategories)

## 📁 Structuring

The structure is based on categories > subcategories > posts > comments. It makes it so that sorting out what the user wants to look for on the website is more facile and clear.

## 🙎‍♂️ User Profile Page

Each user has a profile page that displays information about themselves and their 5 latest posts and comments.

## 🗂️ Posts and comments display

Posts and comments are displayed as lists in their respective pages. The posts are listed in their subcategory, and the comments in their post page.

Posts and comments are also displayed on user profiles (the 5 most recents) and the 5 most recent interracted with posts can be found on the main page.

## ℹ️ Easy access to information

Implemented Rules and FAQ page in order to give users access to an easy way to find out common problems regarding the website and the solutions to them, and in case their problem is not listed on there they can mail us on the Contact page found on the footer.

For problems that need staff action you can find the administration team on the staff page and check their bio for ways to get in touch with them in particular. If they are not reachable that way, the contact form is your best friend.

## 📩 Mailing system for contact

The contact form redirects a mail containing the user id and form data to the administration team in order to help the user with their inquiry. This is achieved using a NuGet Package tackling mailing.

## 👨‍💼 Data management

Users can add posts to subcategories and comments to posts. The activity timing is also tracked, and as such the latest interactions can be seen on the main page.

Posts and comments can be edited/deleted by the user that posted them, as well as be edited/removed by the admins of the website in case of the breach of rules.

Users can edit their profile or get it edited by the admins in case their bio is inappropriate.

## 💼 Admin control

Another extra data management features admins have access to are pinning/unpinning, locking/unlocking posts, removing posts/comments, banning/unbanning users.

## 📄 Pagination

Comments and posts are rendered 10 per page, and for the navigation bar I used MUI resources. Tackled the case of users selecting an invalid number as the page querry, resetting it to 1 if that's the case. When a page with pagination is loaded, the maximum amount of pages is loaded prior to make sure the page the user is on is valid.

## 🏆 Rewards for activity

The users unlock titles they can select when editing their profiles upon reaching certain milestones (currently implemented ones are: 10/25/50 posts/comments) to show off their dedication.

## 📊 Statistics

On the website a lot of statistics are provided regarded the post, users, comments such as the number of currently active/online users, total posts/comments, posts added today, posts/comments added by a user etc.

## ࣪ ִֶָ☾. Simplistic and elegant design

The UI is made with the intention of being intuitive and pleasant to the eye. I combined a dark theme with blue sections in order to give it some color to make it stand out. I feel like I've achieved the minimalist design I was aiming for.

# Roadmap for the future

- Refactoring

- Add more integrations

- Implement image storage and display

- Add rich text

- Add direct messaging features

- Add settings to change the theme and other attributes (maybe to not allow DMs etc.)

# 🏁 Final Thoughts

It was a somewhat long process of learning in which I had the chance to interact with a lot of great people and expand my skill expertise and to even find out more stuff about my way of being such as style of work.

I've not only improved my technical skills, but also my time management and I am really happy about what I have achieved in this time period.
