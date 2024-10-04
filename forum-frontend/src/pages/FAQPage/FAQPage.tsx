import AccordionExpandDefault from "../../components/Accordion/Accordion";
import "./faqpage.css";

const RulesPage = () => {
  return (
    <div className="faq-wrapper">
      <div className="faq-container">
        <div className="title-div">
          <h1>FAQ</h1>
        </div>
        <AccordionExpandDefault title="How do I interact with the content?" content="Navigate to the main page where you can find each category and subcategory related to it. After that, navigate to the desired subcategory that contains posts related to the subject. When you find one that catches your attention, you can navigate through it and interract through a comments." />
        <AccordionExpandDefault title="My content got deleted! Why?" content="In case your content has been deleted it is due to the fact that it breached the content of the website. In case you feel like the decision was not correct contact the administration team in order to discuss the problem." />
        <AccordionExpandDefault title="Will you add X category?" content="Currently we are in a very incipient state, but as the forum progresses we'll think about adding new categories and subcategories. We don't want to go away from the main theming of this forum, which means keeping things tech related." />
      </div>
    </div>
  );
};

export default RulesPage;
