package de.pdinklag.gui;

/**
 * A text field accepting texts up to a certain maximum length.
 */
public class LimitedTextField extends ValueField<String> {
    private static final long serialVersionUID = -1461274951336349105L;

    private int maxLength;

    public LimitedTextField(String text, int maxLength) {
        super(text);
        this.maxLength = maxLength;
    }

    public LimitedTextField(int maxLength) {
        this("", maxLength);
    }

    @Override
    protected String parse(String text) {
        if (text.length() <= maxLength) {
            return text;
        } else {
            return null;
        }
    }
}
