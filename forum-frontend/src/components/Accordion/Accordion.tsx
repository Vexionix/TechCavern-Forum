import Accordion from "@mui/material/Accordion";
import AccordionSummary from "@mui/material/AccordionSummary";
import AccordionDetails from "@mui/material/AccordionDetails";
import Typography from "@mui/material/Typography";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";

import "./accordion.css";

interface Props {
  title: string;
  content: string;
}

export default function AccordionExpandDefault({ title, content }: Props) {
  return (
    <div className="accordion-wrapper">
      <Accordion
        defaultExpanded
        sx={{
          backgroundColor: "#121212",
          border: "1px solid #333",
          "&:before": { display: "none" },
          "&.Mui-expanded": {
            backgroundColor: "#141414",
          },
        }}
      >
        <AccordionSummary
          expandIcon={<ExpandMoreIcon sx={{ color: "#fff" }} />}
          aria-controls="panel1-content"
          id="panel1-header"
        >
          <Typography
            sx={{ fontWeight: "500", fontFamily: "Poppins, sans-serif" }}
          >
            {title}
          </Typography>
        </AccordionSummary>
        <AccordionDetails>
          <Typography
            sx={{
              color: "#D3D3D3",
              fontWeight: "300",
              fontFamily: "Poppins, sans-serif",
            }}
          >
            {content}
          </Typography>
        </AccordionDetails>
      </Accordion>
    </div>
  );
}
