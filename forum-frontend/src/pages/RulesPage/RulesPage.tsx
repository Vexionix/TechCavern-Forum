import "./rulespage.css";

const RulesPage = () => {
  return (
    <div className="rules-wrapper">
      <div className="rules-container">
        <div className="title-div">
          <h1>Rules</h1>
        </div>
        <h2>Useful Information</h2>
        <p>
          This forum's purpose is uniting people with similar interest in the
          tech area and discuss different topics or solve certain problems that
          they encounter. As such we must establish some guidelines so that
          everyone's experience is pleasurable. Breaking the rules may lead to
          warnings and in case of repeated wrongful behaviour users can get
          their account terminated. Ban evading is forbidden and any attempt to
          create a new account will lead to the punishment of it as well. Rules
          can be modified at any point, but the punishment will be based on the
          rules at the moment of the violation.
        </p>

        <h2>Posting rules</h2>
        <ul>
          <li>
            The post must be relevant for the category it has been posted to, as
            such make sure before you post that you chose the correct category.
          </li>
          <li>
            The post should not have malicious intent in any way shape or form
            (this can vary from insulting/harrasing/provoking other forum
            members to posting or sharing malicious websites/software).
          </li>
          <li>The title should be suggestive and not misleading.</li>
          <li>
            The content of the post should be relevant (spam or troll posting is
            not allowed - meme related posts are allowed, mention added to avoid
            ambiguity).
          </li>
        </ul>

        <h2>Commenting rules</h2>
        <ul>
          <li>
            The comment should provide value for the subject of the post,
            posting unrelated comments will lead to their removal.
          </li>
          <li>Harassment is strictly forbbiden.</li>
          <li>
            Editing your comment should provide context to why you've edited it
            and if needed the old content.
          </li>
        </ul>
      </div>
    </div>
  );
};

export default RulesPage;
